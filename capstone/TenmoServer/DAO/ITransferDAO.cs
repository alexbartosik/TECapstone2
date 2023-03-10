using System.Collections.Generic;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDAO
    {
        Transfer CreateSendTransfer(Transfer transfer);
        Transfer CreateRequestTransfer(Transfer transfer);
        List<TransferRecord> ListFromTransfersByUserId(int userId);
        List<TransferRecord> ListToTransfersByUserId(int userId);
        List<TransferRecord> ListPendingTransfersByUserId(int userId);
        TransferRecord GetTransferInfo(int transferId);
        bool SetTransferToRejected(int transferId);
        TransferRecord SetTransferToApproved(int transferId);
    }
}
