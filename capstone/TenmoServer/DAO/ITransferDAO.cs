using System.Collections.Generic;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDAO
    {
        Transfer CreateTransfer(Transfer transfer);
        List<Transfer> ListFromTransfersByUserId(int userId);
        List<Transfer> ListToTransfersByUserId(int userId);
    }
}
