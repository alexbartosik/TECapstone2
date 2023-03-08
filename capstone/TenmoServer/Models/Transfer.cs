using Microsoft.AspNetCore.SignalR;
using System.Security.Principal;
using TenmoServer.DAO;

namespace TenmoServer.Models
{
    public class Transfer
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public int StatusId { get; set; }
        public int AccountFrom { get; set; }
        public int AccountTo { get; set; }
        public decimal Amount { get; set; }
    }
}
