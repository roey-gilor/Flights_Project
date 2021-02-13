﻿using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAO
{
    public class TicketDAOPGSQL : ITicketDAO
    {
        public TicketDAOPGSQL()
        {

        }
        CustomerDAOPGSQL customerDAOPGSQL = new CustomerDAOPGSQL();
        FlightDAOPGSQL flightDAOPGSQL = new FlightDAOPGSQL();
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
            string comand = GetCommand(sp_name, parameters);

            try
            {
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
                Console.WriteLine("cant run sp." + ex.Message);
            }
        }
        private List<Ticket> GetTickets(string conn_string, string sp_name, NpgsqlParameter[] parameters)
        {
            List<Ticket> tickets = new List<Ticket>();
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
                        Ticket ticket = new Ticket
                        {
                            Id = (long)reader["id"],
                            Flight_Id = (long)reader["flight_id"],
                            Customer_Id = (long)reader["customer_id"],
                            Customer = customerDAOPGSQL.Get((long)reader["customer_id"]),
                            Flight = flightDAOPGSQL.Get((long)reader["customer_id"])
                        };
                        tickets.Add(ticket);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("cant run sp." + ex.Message);
            }
            return tickets;
        }
        public void Add(Ticket t)
        {
            RunSpNonExecute(Properties.Resources.Connection_String, "sp_add_ticket", new NpgsqlParameter[]
            {
                new NpgsqlParameter("_flight_id" ,t.Flight_Id),
                new NpgsqlParameter("_customer_id" ,t.Customer_Id)
            });
        }

        public Ticket Get(long id)
        {
            return GetTickets(Properties.Resources.Connection_String, "sp_get_ticket_by_id", new NpgsqlParameter[]
            {
                new NpgsqlParameter("_id" ,id)
            })[0];
        }

        public IList<Ticket> GetAll()
        {
            return GetTickets(Properties.Resources.Connection_String, "sp_get_all_tickets", new NpgsqlParameter[] { });
        }

        public void Remove(Ticket t)
        {
            RunSpNonExecute(Properties.Resources.Connection_String, "sp_remove_ticket", new NpgsqlParameter[]
            {
                new NpgsqlParameter("_id" ,t.Id)
            });
        }

        public void Update(Ticket t)
        {
            RunSpNonExecute(Properties.Resources.Connection_String, "sp_update_ticket", new NpgsqlParameter[]
            {
                new NpgsqlParameter("_id" ,t.Id),
                new NpgsqlParameter("_flight_id", t.Flight_Id),
                new NpgsqlParameter("_customer_id" ,t.Customer_Id)
            });
        }
    }
}
