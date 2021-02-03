using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAO
{
    public class UserDAOPGSQL : IUserDAO
    {
        public UserDAOPGSQL()
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
        private List<User> GetUsers(string conn_string, string sp_name, NpgsqlParameter[] parameters)
        {
            List<User> users = new List<User>();
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
                        User user = new User
                        {
                            Id = (long)reader["id"],
                            User_Name = reader["user_name"].ToString(),
                            Password = reader["password"].ToString(),
                            Email = reader["email"].ToString(),
                            User_Role = (int)reader["user_role"]
                        };
                        users.Add(user);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("cant run sp." + ex.Message);
            }
            return users;
        }
        public void Add(User t)
        {
            RunSpNonExecute(Properties.Resources.Connection_String, "sp_add_user", new NpgsqlParameter[]
            {
                new NpgsqlParameter("_user_name" ,t.User_Name),
                new NpgsqlParameter("_password", t.Password),
                new NpgsqlParameter("_email", t.Email),
                new NpgsqlParameter("_user_role", t.User_Role)
            });
        }

        public User Get(int id)
        {
            return GetUsers(Properties.Resources.Connection_String, "sp_get_user_by_id", new NpgsqlParameter[]
            {
                new NpgsqlParameter("_id" ,id)
            })[0];
        }

        public IList<User> GetAll()
        {
            return GetUsers(Properties.Resources.Connection_String, "sp_get_all_users", new NpgsqlParameter[] { });
        }

        public void Remove(User t)
        {
            RunSpNonExecute(Properties.Resources.Connection_String, "sp_remove_user", new NpgsqlParameter[]
            {
                new NpgsqlParameter("_id" ,t.Id)
            });
        }

        public void Update(User t)
        {
            RunSpNonExecute(Properties.Resources.Connection_String, "sp_update_user", new NpgsqlParameter[]
            {
                new NpgsqlParameter("_id" ,t.Id),
                new NpgsqlParameter("_user_name" ,t.User_Name),
                new NpgsqlParameter("_password", t.Password),
                new NpgsqlParameter("_email", t.Email),
                new NpgsqlParameter("_user_role", t.User_Role)
            });
        }
    }
}
