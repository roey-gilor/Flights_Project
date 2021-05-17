using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAO
{
    public class CustomerDAOPGSQL : ICustomerDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public CustomerDAOPGSQL()
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
        private List<Customer> GetCustomers(string conn_string, string sp_name, NpgsqlParameter[] parameters)
        {
            List<Customer> customers = new List<Customer>();
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
                        Customer customer = new Customer
                        {
                            Id = (long)reader["id"],
                            First_Name = reader["first_name"].ToString(),
                            Last_Name = reader["last_name"].ToString(),
                            Address = reader["address"].ToString(),
                            Phone_No = reader["phone_no"].ToString(),
                            Credit_Card_No = reader["credit_card_no"].ToString(),
                            User_Id = (long)reader["user_id"],
                            User = userDAOPGSQL.Get((long)reader["user_id"])
                        };
                        customers.Add(customer);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"Could not run {sp_name} procedure: {ex.Message}");
            }
            return customers;
        }
        public void Add(Customer t)
        {
            RunSpNonExecute(AppConfig.Instance.ConnectionString, "sp_add_customer", new NpgsqlParameter[]
            {
                new NpgsqlParameter("_first_name" ,t.First_Name),
                new NpgsqlParameter("_last_name" ,t.Last_Name),
                new NpgsqlParameter("_address" ,t.Address),
                new NpgsqlParameter("_phone_no" ,t.Phone_No),
                new NpgsqlParameter("_credit_card_no" ,t.Credit_Card_No),
                new NpgsqlParameter("_user_id" ,t.User_Id)
            });
        }

        public Customer Get(long id)
        {
            return GetCustomers(AppConfig.Instance.ConnectionString, "sp_get_customer_by_id", new NpgsqlParameter[]
            {
                new NpgsqlParameter("_id" ,id)
            })[0];
        }

        public IList<Customer> GetAll()
        {
            return GetCustomers(AppConfig.Instance.ConnectionString, "sp_Get_All_Customers", new NpgsqlParameter[] { });
        }

        public Customer GetCustomerByUserame(string name)
        {
            return GetCustomers(AppConfig.Instance.ConnectionString, "sp_get_customer_by_user_name", new NpgsqlParameter[]
            {
                new NpgsqlParameter("_user_name" ,name)
            })[0];
        }

        public void Remove(Customer t)
        {
            RunSpNonExecute(AppConfig.Instance.ConnectionString, "sp_remove_customer", new NpgsqlParameter[]
            {
                new NpgsqlParameter("_id" ,t.Id)
            });
        }

        public void Update(Customer t)
        {
            RunSpNonExecute(AppConfig.Instance.ConnectionString, "sp_update_customer", new NpgsqlParameter[]
            {
                new NpgsqlParameter("_id" ,t.Id),
                new NpgsqlParameter("_first_name" ,t.First_Name),
                new NpgsqlParameter("_last_name" ,t.Last_Name),
                new NpgsqlParameter("_address" ,t.Address),
                new NpgsqlParameter("_phone_no" ,t.Phone_No),
                new NpgsqlParameter("_credit_card_no" ,t.Credit_Card_No),
                new NpgsqlParameter("_user_id" ,t.User_Id)
            });
        }

        public Customer GetCustomerByUserId(long id)
        {
            return GetCustomers(AppConfig.Instance.ConnectionString, "sp_get_customer_by_user_id", new NpgsqlParameter[]
            {
                new NpgsqlParameter("_userId" ,id)
            })[0];
        }
    }
}
