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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public CountryDAOPGSQL()
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
        private void RunSpNonExecute (string conn_string, string sp_name, NpgsqlParameter[] parameters)
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
                log.Error($"Could not run {sp_name} procedure: {ex.Message}");
            }
            return countries;
        }
        public long Add(Country t)
        {
            try
            {
                RunSpNonExecute(AppConfig.Instance.ConnectionString, "sp_add_country", new NpgsqlParameter[]
                {
                new NpgsqlParameter("_name" ,t.Name)
                });
                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Country Get(long id)
        {
            return GetCountries(AppConfig.Instance.ConnectionString, "sp_get_country_by_id", new NpgsqlParameter[]
            {
                new NpgsqlParameter("_id" ,id)
            })[0];
        }

        public IList<Country> GetAll()
        {
            return GetCountries(AppConfig.Instance.ConnectionString, "sp_Get_All_Countries", new NpgsqlParameter[] { });
        }

        public void Remove(Country t)
        {
            RunSpNonExecute(AppConfig.Instance.ConnectionString, "sp_remove_country", new NpgsqlParameter[]
            {
                new NpgsqlParameter("_id" ,t.Id)
            });
        }

        public void Update(Country t)
        {
            try
            {
                RunSpNonExecute(AppConfig.Instance.ConnectionString, "sp_update_country", new NpgsqlParameter[]
                {
                new NpgsqlParameter("_id" ,t.Id),
                new NpgsqlParameter("_name", t.Name)
                });
            }
            catch (Exception  ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
