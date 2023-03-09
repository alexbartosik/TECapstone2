using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class TransferSqlDAO:ITransferDAO
    {
        private readonly string connectionString;
        public TransferSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public Transfer CreateTransfer(Transfer transfer)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount) VALUES (1001, 2001, (SELECT account_id FROM accounts WHERE user_id = @accountFrom), (SELECT account_id FROM accounts WHERE user_id = @accountTo), @amount); SELECT @@IDENTITY";

                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddWithValue("@accountFrom", transfer.AccountFrom);
                command.Parameters.AddWithValue("@accountTo", transfer.AccountTo);
                command.Parameters.AddWithValue("@amount", transfer.Amount);

                transfer.Id = Convert.ToInt32(command.ExecuteScalar());

                return transfer;
            }
        }

        public List<Transfer> ListFromTransfersByUserId(int userId)
        {
            List<Transfer> transfers = new List<Transfer>();

            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM transfers WHERE account_from = (SELECT account_id FROM accounts WHERE user_id = @userId)";

                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddWithValue("@userId", userId);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Transfer transfer = new Transfer();

                    transfer.Id = Convert.ToInt32(reader["transfer_id"]);
                    transfer.TypeId = Convert.ToInt32(reader["transfer_type_id"]);
                    transfer.StatusId = Convert.ToInt32(reader["transfer_status_id"]);
                    transfer.AccountFrom = Convert.ToInt32(reader["account_from"]);
                    transfer.AccountTo = Convert.ToInt32(reader["account_to"]);
                    transfer.Amount = Convert.ToDecimal(reader["amount"]);

                    transfers.Add(transfer);
                }

                return transfers;
            }

        }

        public List<Transfer> ListToTransfersByUserId(int userId)
        {
            List<Transfer> transfers = new List<Transfer>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM transfers WHERE account_to = (SELECT account_id FROM accounts WHERE user_id = @userId)";

                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddWithValue("@userId", userId);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Transfer transfer = new Transfer();

                    transfer.Id = Convert.ToInt32(reader["transfer_id"]);
                    transfer.TypeId = Convert.ToInt32(reader["transfer_type_id"]);
                    transfer.StatusId = Convert.ToInt32(reader["transfer_status_id"]);
                    transfer.AccountFrom = Convert.ToInt32(reader["account_from"]);
                    transfer.AccountTo = Convert.ToInt32(reader["account_to"]);
                    transfer.Amount = Convert.ToDecimal(reader["amount"]);

                    transfers.Add(transfer);
                }

                return transfers;
            }
        }
    }
}
