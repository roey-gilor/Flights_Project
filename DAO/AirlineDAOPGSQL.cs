using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAO
{
    public class AirlineDAOPGSQL : IAirlineDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        UserDAOPGSQL user = new UserDAOPGSQL();
        public AirlineDAOPGSQL()
        {

        }
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
                throw new Exception(ex.Message);
            }
        }
        private List<AirlineCompany> GetAirlineCompanies(string conn_string, string sp_name, NpgsqlParameter[] parameters)
        {
            List<AirlineCompany> airline_companies = new List<AirlineCompany>();
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
                        AirlineCompany airlineCompany = new AirlineCompany
                        {
                            Id = (long)reader["id"],
                            Name = reader["name"].ToString(),
                            Country_Id = (long)reader["country_id"],
                            User_Id = (long)reader["user_id"],
                            User = user.Get((long)reader["user_id"])
                        };
                        airline_companies.Add(airlineCompany);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"Could not run {sp_name} procedure: {ex.Message}");
            }
            return airline_companies;
        }
        public void Add(AirlineCompany t)
        {
            try
            {
                RunSpNonExecute(AppConfig.Instance.ConnectionString, "sp_add_airline", new NpgsqlParameter[]
                {
                new NpgsqlParameter("_name" ,t.Name),
                new NpgsqlParameter("_country_id",t.Country_Id),
                new NpgsqlParameter("_user_id",t.User_Id)
                });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public AirlineCompany Get(long id)
        {
            return GetAirlineCompanies(AppConfig.Instance.ConnectionString, "sp_get_airline_by_id", new NpgsqlParameter[]
            {
                new NpgsqlParameter("_id" ,id)
            })[0];
        }

        public AirlineCompany GetAirlineByUserame(string name)
        {
            return GetAirlineCompanies(AppConfig.Instance.ConnectionString, "sp_get_airline_by_user_name", new NpgsqlParameter[]
            {
                new NpgsqlParameter("_user_name" ,name)
            })[0];
        }

        public IList<AirlineCompany> GetAll()
        {
            return GetAirlineCompanies(AppConfig.Instance.ConnectionString, "sp_get_all_airlines", new NpgsqlParameter[] { });
        }

        public IList<AirlineCompany> GetAllAirlinesByCountry(int countryId)
        {
            return GetAirlineCompanies(AppConfig.Instance.ConnectionString, "sp_get_airline_by_country", new NpgsqlParameter[]
            {
                new NpgsqlParameter("_country_id" ,countryId)
            });
        }

        public void Remove(AirlineCompany t)
        {
            RunSpNonExecute(AppConfig.Instance.ConnectionString, "sp_remove_airline", new NpgsqlParameter[]
            {
                new NpgsqlParameter("_id" ,t.Id)
            });
        }

        public void Update(AirlineCompany t)
        {
            try
            {
                RunSpNonExecute(AppConfig.Instance.ConnectionString, "sp_update_airline", new NpgsqlParameter[]
                {
                new NpgsqlParameter("_id" ,t.Id),
                new NpgsqlParameter("_name" ,t.Name),
                new NpgsqlParameter("_country_id",t.Country_Id),
                new NpgsqlParameter("_user_id",t.User_Id)
                });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public AirlineCompany GetAirlineByUserId(long id)
        {
            return GetAirlineCompanies(AppConfig.Instance.ConnectionString, "sp_get_airline_by_user_id", new NpgsqlParameter[]
            {
                new NpgsqlParameter("_userid" ,id)
            })[0];
        }
    }
}
