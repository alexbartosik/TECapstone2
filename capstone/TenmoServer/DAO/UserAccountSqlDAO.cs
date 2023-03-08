﻿using System;
using System.Data.SqlClient;
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
            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = "SELECT a.balance FROM accounts a WHERE a.user_id = (SELECT u.user_id FROM users u WHERE u.username = @username)";

                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddWithValue("@username", username);

                decimal balance = Convert.ToDecimal(command.ExecuteScalar());

                return balance;
            }
        }
    }
}