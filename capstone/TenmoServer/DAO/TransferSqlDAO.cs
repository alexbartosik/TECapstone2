using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
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

        public Transfer CreateSendTransfer(Transfer transfer)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                const int sendType = 1001;
                const int sendDefaultStatusApproved = 2001;

                conn.Open();
                string sql = "INSERT INTO transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount) VALUES (@type, @status, (SELECT account_id FROM accounts WHERE user_id = @accountFrom), (SELECT account_id FROM accounts WHERE user_id = @accountTo), @amount); SELECT @@IDENTITY";

                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddWithValue("@accountFrom", transfer.AccountFrom);
                command.Parameters.AddWithValue("@accountTo", transfer.AccountTo);
                command.Parameters.AddWithValue("@amount", transfer.Amount);
                command.Parameters.AddWithValue("@type", sendType);
                command.Parameters.AddWithValue("@status", sendDefaultStatusApproved);

                transfer.Id = Convert.ToInt32(command.ExecuteScalar());
                transfer.TypeId = sendType;
                transfer.StatusId = sendDefaultStatusApproved;

                return transfer;
            }
        }

        public Transfer CreateRequestTransfer(Transfer transfer)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                const int requestType = 1000;
                const int requestDefaultStatusPending = 2000;

                conn.Open();
                string sql = "INSERT INTO transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount) VALUES (@type, @status, (SELECT account_id FROM accounts WHERE user_id = @accountFrom), (SELECT account_id FROM accounts WHERE user_id = @accountTo), @amount); SELECT @@IDENTITY";

                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddWithValue("@accountFrom", transfer.AccountFrom);
                command.Parameters.AddWithValue("@accountTo", transfer.AccountTo);
                command.Parameters.AddWithValue("@amount", transfer.Amount);
                command.Parameters.AddWithValue("@type", requestType);
                command.Parameters.AddWithValue("@status", requestDefaultStatusPending);

                transfer.Id = Convert.ToInt32(command.ExecuteScalar());
                transfer.TypeId = requestType;
                transfer.StatusId = requestDefaultStatusPending;

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
                    transfer.ToName = Convert.ToString(reader["username"]);
                    transfer.FromName = "";
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
                    transfer.FromName = Convert.ToString(reader["username"]);
                    transfer.ToName = "";
                    transfer.TransferDirection = "From: ";
                    transfer.Amount = Convert.ToDecimal(reader["amount"]);

                    fromTransfers.Add(transfer);
                }

                return fromTransfers;
            }
        }

        public List<TransferRecord> ListPendingTransfersByUserId(int userId)
        {
            List<TransferRecord> pendingTransfers = new List<TransferRecord>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT t.transfer_id, t.amount, u.username FROM transfers t JOIN accounts a ON a.account_id = t.account_to JOIN users u ON u.user_id = a.user_id WHERE account_from = (SELECT account_id FROM accounts WHERE user_id = @userId) AND transfer_status_id = 2000";

                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddWithValue("@userId", userId);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    TransferRecord transfer = new TransferRecord();

                    transfer.Id = Convert.ToInt32(reader["transfer_id"]);
                    transfer.ToName = Convert.ToString(reader["username"]);
                    transfer.Amount = Convert.ToDecimal(reader["amount"]);

                    pendingTransfers.Add(transfer);
                }

                return pendingTransfers;
            }
        }

        public TransferRecord GetTransferInfo(int transferId)
        {
            TransferRecord transfer = new TransferRecord();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = "SELECT t.transfer_id, tt.transfer_type_desc, ts.transfer_status_desc, t.amount, u.username AS 'From User', v.username AS 'To User' FROM transfers t JOIN accounts a ON a.account_id = t.account_from JOIN accounts b ON b.account_id = t.account_to JOIN users u ON u.user_id = a.user_id JOIN users v ON v.user_id = b.user_id JOIN transfer_statuses ts ON ts.transfer_status_id = t.transfer_status_id JOIN transfer_types tt ON tt.transfer_type_id = t.transfer_type_id WHERE transfer_id = @transferId";
                // SQL statement joins all tables to grab all transfer information, usernames and descriptions for display

                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddWithValue("@transferId", transferId);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    transfer.Id = transferId;
                    transfer.FromName = Convert.ToString(reader["From User"]);
                    transfer.ToName = Convert.ToString(reader["To User"]);
                    transfer.TypeId = Convert.ToString(reader["transfer_type_desc"]);
                    transfer.StatusId = Convert.ToString(reader["transfer_status_desc"]);
                    transfer.Amount = Convert.ToDecimal(reader["amount"]);
                }
                return transfer;
            }
        }
    }
}
