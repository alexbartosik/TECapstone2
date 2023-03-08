using System.Collections.Generic;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface IUserAccountDAO
    {
        decimal GetMyAccountBalance(string username);
        bool IncreaseAccountBalance(int userId, decimal amountToAdd);
        bool DecreaseAccountBalance(int userId, decimal amountToSubtract);
        List<User> GetUsers(string username);
    }
}
