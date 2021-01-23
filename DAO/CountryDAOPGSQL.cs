using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace DAO
{
    public class CountryDAOPGSQL : ICountryDAO
    {
        public CountryDAOPGSQL()
        {

        }
        private void RunSpNonExecute (string conn_string, string sp_name, NpgsqlParameter[] parameters)
        {
            try
            {
                using (var conn = new NpgsqlConnection(conn_string))
                {
                    conn.Open();
                    NpgsqlCommand command = new NpgsqlCommand(sp_name, conn);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("cant run sp." + ex.Message);
            }
        }
        private List<Country> GetCountries(string conn_string, string sp_name, NpgsqlParameter[] parameters)
        {
            List<Country> countries = new List<Country>();
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
                        Country country = new Country
                        {
                            Id = (long)reader["id"],
                            Name = reader["name"].ToString()
                        };
                        countries.Add(country);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("cant run sp." + ex.Message);
            }
            return countries;
        }
        void IBasicDB<Country>.Add(Country t)
        {
            RunSpNonExecute(Properties.Resources.Connection_String, "sp_add_country", new NpgsqlParameter[]
            {
                new NpgsqlParameter("name" ,t.Name)
            });
        }

        Country IBasicDB<Country>.Get(int id)
        {
            return GetCountries(Properties.Resources.Connection_String, "sp_Get_Country_By_Id", new NpgsqlParameter[]
            {
                new NpgsqlParameter("id" ,id)
            })[0];
        }

        IList<Country> IBasicDB<Country>.GetAll()
        {
            return GetCountries(Properties.Resources.Connection_String, "sp_Get_All_Countries", new NpgsqlParameter[] { });
        }

        void IBasicDB<Country>.Remove(Country t)
        {
            RunSpNonExecute(Properties.Resources.Connection_String, "sp_Remove_Country", new NpgsqlParameter[]
            {
                new NpgsqlParameter("id" ,t.Id)
            });
        }

        void IBasicDB<Country>.Update(Country t)
        {
            RunSpNonExecute(Properties.Resources.Connection_String, "sp_Update_Country", new NpgsqlParameter[]
            {
                new NpgsqlParameter("id" ,t.Id),
                new NpgsqlParameter("Name", t.Name)
            });
        }
    }
}
