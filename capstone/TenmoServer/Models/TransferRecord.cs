namespace TenmoServer.Models
{
    public class TransferRecord
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TransferDirection { get; set; }
        public decimal Amount { get; set; }
    }
}
