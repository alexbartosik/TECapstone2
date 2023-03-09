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

        public List<TransferRecord> ListToTransfersByUserId(int userId)
        {
            List<TransferRecord> toTransfers = new List<TransferRecord>();

            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT t.transfer_id, t.amount, u.username FROM transfers t JOIN accounts a ON a.account_id = t.account_to JOIN users u ON u.user_id = a.user_id WHERE account_from = (SELECT account_id FROM accounts WHERE user_id = @userId)";

                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddWithValue("@userId", userId);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    TransferRecord transfer = new TransferRecord();

                    transfer.Id = Convert.ToInt32(reader["transfer_id"]);
                    transfer.Name = Convert.ToString(reader["username"]);
                    transfer.TransferDirection = "To: ";
                    transfer.Amount = Convert.ToDecimal(reader["amount"]);

                    toTransfers.Add(transfer);
                }

                return toTransfers;
            }

        }

        public List<TransferRecord> ListFromTransfersByUserId(int userId)
        {
            List<TransferRecord> fromTransfers = new List<TransferRecord>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT t.transfer_id, t.amount, u.username FROM transfers t JOIN accounts a ON a.account_id = t.account_from JOIN users u ON u.user_id = a.user_id WHERE account_to = (SELECT account_id FROM accounts WHERE user_id = @userId)";

                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddWithValue("@userId", userId);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    TransferRecord transfer = new TransferRecord();

                    transfer.Id = Convert.ToInt32(reader["transfer_id"]);
                    transfer.Name = Convert.ToString(reader["username"]);
                    transfer.TransferDirection = "From: ";
                    transfer.Amount = Convert.ToDecimal(reader["amount"]);

                    fromTransfers.Add(transfer);
                }

                return fromTransfers;
            }
        }
    }
}
