using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface IUserAccountDAO
    {
        decimal GetMyAccountBalance(string username);
    }
}
