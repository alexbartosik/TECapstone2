using System;
using System.ComponentModel.DataAnnotations;

namespace TenmoClient.Models
{
    public class Transfer
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int TypeId { get; set; }
        [Required]
        public int StatusId { get; set; }
        [Required]
        public int AccountFrom { get; set; }
        [Required]
        public int AccountTo { get; set; }
        [Required]
        public decimal Amount { get; set; }
    }
}
