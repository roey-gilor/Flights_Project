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
        public void Add(Administrator t)
        {
            RunSpNonExecute(AppConfig.Instance.ConnectionString, "sp_add_admin", new NpgsqlParameter[]
            {
                new NpgsqlParameter("_first_name", t.First_Name),
                new NpgsqlParameter("_last_name", t.Last_Name),
                new NpgsqlParameter("_level", t.Level),
                new NpgsqlParameter("_user_id", t.User_Id)
            });
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
    }
}
