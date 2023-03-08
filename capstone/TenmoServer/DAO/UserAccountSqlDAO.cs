using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Cryptography.Xml;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class UserAccountSqlDAO : IUserAccountDAO
    {
        private readonly string connectionString;
        public UserAccountSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public decimal GetMyAccountBalance(string username)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = "SELECT a.balance FROM accounts a WHERE a.user_id = (SELECT u.user_id FROM users u WHERE u.username = @username)";

                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddWithValue("@username", username);

                decimal balance = Convert.ToDecimal(command.ExecuteScalar());

                return balance;
            }
        }

        public bool IncreaseAccountBalance(int userId, decimal amountToAdd)
        {
            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = "UPDATE accounts SET balance += @amountToAdd WHERE user_id = @userId";

                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@amountToAdd", amountToAdd);

                if (command.ExecuteNonQuery() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool DecreaseAccountBalance(int userId, decimal amountToSubtract)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = "UPDATE accounts SET balance -= @amountToSubtract WHERE user_id = @userId";

                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@amountToSubtract", amountToSubtract);

                if (command.ExecuteNonQuery() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<User> GetUsers(string username)
        {
            List<User> returnUsers = new List<User>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT user_id, username FROM users WHERE username != @username", conn);
                cmd.Parameters.AddWithValue("@username", username);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    User user = new User();

                    user.Username = Convert.ToString(reader["username"]);
                    user.UserId = Convert.ToInt32(reader["user_id"]);

                    returnUsers.Add(user);
                }
            }

            return returnUsers;
        }
    }
}
