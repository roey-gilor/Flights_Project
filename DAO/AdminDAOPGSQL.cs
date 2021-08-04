using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;

namespace DAO
{
    public class AdminDAOPGSQL : IAdminDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public AdminDAOPGSQL()
        { 

        }
        UserDAOPGSQL userDAOPGSQL = new UserDAOPGSQL();
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
        private List<Administrator> GetAdministrators(string conn_string, string sp_name, NpgsqlParameter[] parameters)
        {
            List<Administrator> administrators = new List<Administrator>();
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
                        Administrator administrator = new Administrator
                        {
                            Id = (long)reader["id"],
                            First_Name = reader["first_name"].ToString(),
                            Last_Name = reader["last_name"].ToString(),
                            Level = (int)reader["level"],
                            User_Id = (long)reader["user_id"],
                            User = userDAOPGSQL.Get((long)reader["user_id"])
                        };
                        administrators.Add(administrator);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"Could not run {sp_name} procedure: {ex.Message}");
            }
            return administrators;
        }
        public long Add(Administrator t)
        {
            long id = 0;
            NpgsqlParameter[] parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("_first_name", t.First_Name),
                new NpgsqlParameter("_last_name", t.Last_Name),
                new NpgsqlParameter("_level", t.Level),
                new NpgsqlParameter("_user_id", t.User_Id)
            };
            using (var conn = new NpgsqlConnection(AppConfig.Instance.ConnectionString))
            {
                conn.Open();

                NpgsqlCommand command = new NpgsqlCommand("sp_add_admin", conn);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddRange(parameters);

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    id = (long)reader[0];
                }
            }
            return id;
        }

        public Administrator Get(long id)
        {
            return GetAdministrators(AppConfig.Instance.ConnectionString, "sp_get_admin_by_id", new NpgsqlParameter[]
            {
                new NpgsqlParameter("_id" ,id)
            })[0];
        }

        public IList<Administrator> GetAll()
        {
            return GetAdministrators(AppConfig.Instance.ConnectionString, "sp_get_all_admins", new NpgsqlParameter[] { });
        }

        public void Remove(Administrator t)
        {
            RunSpNonExecute(AppConfig.Instance.ConnectionString, "sp_remove_admin", new NpgsqlParameter[]
            {
                new NpgsqlParameter("_id" ,t.Id)
            });
        }

        public void Update(Administrator t)
        {
            RunSpNonExecute(AppConfig.Instance.ConnectionString, "sp_update_admin", new NpgsqlParameter[]
            {
                new NpgsqlParameter("_id" ,t.Id),
                new NpgsqlParameter("_first_name", t.First_Name),
                new NpgsqlParameter("_last_name", t.Last_Name),
                new NpgsqlParameter("_level", t.Level),
                new NpgsqlParameter("_user_id", t.User_Id)
            });
        }

        public Administrator GetAdminByUserId(long id)
        {
            return GetAdministrators(AppConfig.Instance.ConnectionString, "sp_get_admin_by_user_id", new NpgsqlParameter[]
            {
                new NpgsqlParameter("_userid" ,id)
            })[0];
        }
        public IList<AirlineCompany> GetAllWaitingAirlines()
        {
            List<AirlineCompany> airline_companies = new List<AirlineCompany>();
            try
            {
                using (var conn = new NpgsqlConnection(AppConfig.Instance.ConnectionString))
                {
                    conn.Open();

                    NpgsqlCommand command = new NpgsqlCommand("sp_get_all_waiting_airlines", conn);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        AirlineCompany airlineCompany = new AirlineCompany
                        {
                            Id = (long)reader["id"],
                            Name = reader["airline_name"].ToString(),
                            Country_Id = (long)reader["country_id"],
                            User = new User
                            {
                                User_Name = reader["username"].ToString(),
                                Password = reader["password"].ToString(),
                                Email = reader["email"].ToString()
                            }
                        };
                        airline_companies.Add(airlineCompany);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"Could not get all waiting airlines: {ex.Message}");
            }
            return airline_companies;
        }

        public void RemoveWaitingAirline(AirlineCompany airlineCompany)
        {
            RunSpNonExecute(AppConfig.Instance.ConnectionString, "sp_remove_waiting_airline", new NpgsqlParameter[]
            {
                new NpgsqlParameter("_id" ,airlineCompany.Id)
            });
        }

        public long AddWaitingAdmin(Administrator administrator)
        {
            try
            {
                long id = 0;
                NpgsqlParameter[] parameters = new NpgsqlParameter[]
                {
                new NpgsqlParameter("_first_name" ,administrator.First_Name),
                new NpgsqlParameter("_last_name",administrator.Last_Name),
                new NpgsqlParameter("_level",administrator.Level),
                new NpgsqlParameter("_username",administrator.User.User_Name),
                new NpgsqlParameter("_password",administrator.User.Password),
                new NpgsqlParameter("_email",administrator.User.Email)
                };
                using (var conn = new NpgsqlConnection(AppConfig.Instance.ConnectionString))
                {
                    conn.Open();

                    NpgsqlCommand command = new NpgsqlCommand("sp_add_waiting_admin", conn);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddRange(parameters);

                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        id = (long)reader[0];
                    }
                }
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IList<Administrator> GetAllWaitingAdmins()
        {
            List<Administrator> administrators = new List<Administrator>();
            try
            {
                using (var conn = new NpgsqlConnection(AppConfig.Instance.ConnectionString))
                {
                    conn.Open();

                    NpgsqlCommand command = new NpgsqlCommand("sp_get_all_waiting_admins", conn);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Administrator administrator = new Administrator
                        {
                            Id = (long)reader["id"],
                            First_Name = reader["first_name"].ToString(),
                            Last_Name = reader["last_name"].ToString(),
                            Level = (int)reader["level"],
                            User = new User
                            {
                                User_Name = reader["username"].ToString(),
                                Password = reader["password"].ToString(),
                                Email = reader["email"].ToString()
                            }
                        };
                        administrators.Add(administrator);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"Could not get all waiting airlines: {ex.Message}");
            }
            return administrators;
        }

        public void RemoveWaitingAdmin(Administrator administrator)
        {
            RunSpNonExecute(AppConfig.Instance.ConnectionString, "sp_remove_waiting_admin", new NpgsqlParameter[]
            {
                new NpgsqlParameter("_id" ,administrator.Id)
            });
        }
    }
}
