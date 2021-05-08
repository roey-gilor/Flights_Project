using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAO
{
    public class FlightDAOPGSQL : IFlightDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public FlightDAOPGSQL()
        {

        }
        AirlineDAOPGSQL airlineDAOPGSQL = new AirlineDAOPGSQL();
        private string GetCommand(string sp_name, NpgsqlParameter[] parameters)
        {
            string comand = $"CALL {sp_name} (";
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].Value.GetType() == typeof(int) || parameters[i].Value.GetType() == typeof(long))
                {
                    if (i != parameters.Length - 1)
                        comand += $" {parameters[i].Value},";
                    else
                        comand += $" {parameters[i].Value}";
                }
                else
                    if (parameters[i].Value.GetType() == typeof(string) || parameters[i].Value.GetType() == typeof(DateTime))
                {
                    if (i != parameters.Length - 1)
                        comand += $" '{parameters[i].Value}',";
                    else
                        comand += $" '{parameters[i].Value}'";
                }
            }
            comand += $")";
            return comand;
        }
        private void RunSpNonExecute(string conn_string, string sp_name, NpgsqlParameter[] parameters)
        {
            try
            {
                string comand = GetCommand(sp_name, parameters);
                using (var conn = new NpgsqlConnection(conn_string))
                {
                    conn.Open();
                    NpgsqlCommand command = new NpgsqlCommand(sp_name, conn);
                    command.CommandText = comand;
                    command.Parameters.AddRange(parameters);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log.Error($"Could not run {sp_name} procedure: {ex.Message}");
            }
        }
        private List<Flight> GetFlights(string conn_string, string sp_name, NpgsqlParameter[] parameters)
        {
            List<Flight> flights = new List<Flight>();
            try
            {
                using (var conn = new NpgsqlConnection(conn_string))
                {
                    conn.Open();

                    NpgsqlCommand command = new NpgsqlCommand(sp_name, conn);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddRange(parameters);

                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Flight flight = new Flight
                        {
                            Id = (long)reader["id"],
                            Airline_Company_Id = (long)reader["airline_company_id"],
                            Origin_Country_Id = (long)reader["origin_country_id"],
                            Destination_Country_Id = (long)reader["destination_country_id"],
                            Departure_Time = Convert.ToDateTime(reader["departure_time"]),
                            Landing_Time = Convert.ToDateTime(reader["landing_time"]),
                            Remaining_Tickets = (int)reader["remaining_tickets"],
                            Airline_Company = airlineDAOPGSQL.Get((long)reader["airline_company_id"])
                        };
                        flights.Add(flight);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"Could not run {sp_name} procedure: {ex.Message}");
            }
            return flights;
        }
        public void Add(Flight t)
        {
            RunSpNonExecute(AppConfig.Instance.ConnectionString, "sp_add_flight", new NpgsqlParameter[]
            {
                new NpgsqlParameter("_airline_company_id" ,t.Airline_Company_Id),
                new NpgsqlParameter("_origin_country_id" ,t.Origin_Country_Id),
                new NpgsqlParameter("_destination_country_id" ,t.Destination_Country_Id),
                new NpgsqlParameter("_departure_time" ,t.Departure_Time),
                new NpgsqlParameter("_landing_time" ,t.Landing_Time),
                new NpgsqlParameter("_remaining_tickets" ,t.Remaining_Tickets),
            });
        }

        public Flight Get(long id)
        {
            return GetFlights(AppConfig.Instance.ConnectionString, "sp_get_flight_by_id", new NpgsqlParameter[]
            {
                new NpgsqlParameter("_id", id)
            })[0];
        }

        public IList<Flight> GetAll()
        {
            return GetFlights(AppConfig.Instance.ConnectionString, "sp_get_all_flights", new NpgsqlParameter[] { });
        }

        public Dictionary<Flight, int> GetAllFlightsVacancy()
        {
            Dictionary<Flight, int> flights_vacancy = new Dictionary<Flight, int>();
            try
            {
                using (var conn = new NpgsqlConnection(AppConfig.Instance.ConnectionString))
                {
                    conn.Open();

                    NpgsqlCommand command = new NpgsqlCommand("sp_get_all_vacancy_flights", conn);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int remaining_tickets = (int)reader["remaining_tickets"];
                        Flight flight = new Flight
                        {
                            Id = (long)reader["id"],
                            Airline_Company_Id = (long)reader["airline_company_id"],
                            Origin_Country_Id = (long)reader["origin_country_id"],
                            Destination_Country_Id = (long)reader["destination_country_id"],
                            Departure_Time = Convert.ToDateTime(reader["departure_time"]),
                            Landing_Time = Convert.ToDateTime(reader["landing_time"]),
                            Remaining_Tickets = (int)reader["remaining_tickets"],
                            Airline_Company = airlineDAOPGSQL.Get((long)reader["airline_company_id"])
                        };
                        flights_vacancy.Add(flight, remaining_tickets);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"Could not run Get All Flights Vacancy procedure: {ex.Message}");
            }
            return flights_vacancy;
        }

        public IList<Flight> GetFlightsByCustomer(Customer customer)
        {
            return GetFlights(AppConfig.Instance.ConnectionString, "sp_get_flight_by_customer", new NpgsqlParameter[]
            {
                new NpgsqlParameter("_customer_id", customer.Id)
            });
        }

        public IList<Flight> GetFlightsByDepatrureDate(DateTime departureDate)
        {
            return GetFlights(AppConfig.Instance.ConnectionString, "sp_get_flights_by_departure_date", new NpgsqlParameter[]
            {
                new NpgsqlParameter("_departure_time", departureDate)
            });
        }

        public IList<Flight> GetFlightsByOriginCountry(int countryCode)
        {
            return GetFlights(AppConfig.Instance.ConnectionString, "sp_get_flights_by_origin_country", new NpgsqlParameter[]
            {
                new NpgsqlParameter("_country_code", countryCode)
            });
        }

        public IList<Flight> GetFlightsByDestinationCountry(int countryCode)
        {
            return GetFlights(AppConfig.Instance.ConnectionString, "sp_get_flights_by_destination_country", new NpgsqlParameter[]
            {
                new NpgsqlParameter("_country_code", countryCode)
            });
        }

        public IList<Flight> GetFlightsByLandingDate(DateTime landingDate)
        {
            return GetFlights(AppConfig.Instance.ConnectionString, "sp_get_flights_by_landing_date", new NpgsqlParameter[]
            {
                new NpgsqlParameter("_landing_time", landingDate)
            });
        }

        public void Remove(Flight t)
        {
            RunSpNonExecute(AppConfig.Instance.ConnectionString, "sp_remove_flight", new NpgsqlParameter[]
            {
                new NpgsqlParameter("_id" ,t.Id)
            });
        }

        public void Update(Flight t)
        {
            RunSpNonExecute(AppConfig.Instance.ConnectionString, "sp_update_flight", new NpgsqlParameter[]
            {
                new NpgsqlParameter("_id" ,t.Id),
                new NpgsqlParameter("_airline_company_id" ,t.Airline_Company_Id),
                new NpgsqlParameter("_origin_country_id" ,t.Origin_Country_Id),
                new NpgsqlParameter("_destination_country_id" ,t.Destination_Country_Id),
                new NpgsqlParameter("_departure_time" ,t.Departure_Time),
                new NpgsqlParameter("_landing_time" ,t.Landing_Time),
                new NpgsqlParameter("_remaining_tickets" ,t.Remaining_Tickets),
            });
        }

        public IList<Flight> GetOldFlights(DateTime landingDate)
        {
            return GetFlights(AppConfig.Instance.ConnectionString, "sp_get_flights_before_datetime", new NpgsqlParameter[]
            {
                new NpgsqlParameter("_landing_time", landingDate)
            });
        }

        public void Add_to_flights_history(Flight flight)
        {
            RunSpNonExecute(AppConfig.Instance.ConnectionString, "sp_add_flight_into_flights_history", new NpgsqlParameter[]
            {
                new NpgsqlParameter("_flight_id",flight.Id),
                new NpgsqlParameter("_airline_company_id" ,flight.Airline_Company_Id),
                new NpgsqlParameter("_origin_country_id" ,flight.Origin_Country_Id),
                new NpgsqlParameter("_destination_country_id" ,flight.Destination_Country_Id),
                new NpgsqlParameter("_departure_time" ,flight.Departure_Time),
                new NpgsqlParameter("_landing_time" ,flight.Landing_Time),
                new NpgsqlParameter("_remaining_tickets" ,flight.Remaining_Tickets),
            });
        }
    }
}
