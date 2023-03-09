using System.Collections.Generic;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDAO
    {
        Transfer CreateTransfer(Transfer transfer);
        List<TransferRecord> ListFromTransfersByUserId(int userId);
        List<TransferRecord> ListToTransfersByUserId(int userId);
        TransferRecord GetTransferInfo(int transferId);
    }
}
