using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface IUserAccountDAO
    {
        decimal GetMyAccountBalance(string username);
        bool IncreaseAccountBalance(int userId, decimal amountToAdd);
        bool DecreaseAccountBalance(string username, decimal amountToSubtract);
    }
}
