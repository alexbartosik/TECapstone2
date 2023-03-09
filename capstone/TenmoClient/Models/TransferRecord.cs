namespace TenmoClient.Models
{
    public class TransferRecord
    {
        public int Id { get; set; }
        public string FromName { get; set; }
        public string ToName { get; set; }
        public string TypeId { get; set; }
        public string StatusId { get; set; }
        public string TransferDirection { get; set; }
        public decimal Amount { get; set; }
    }
}
