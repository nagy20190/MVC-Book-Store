using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BKStore_MVC.Models;

namespace BKStore_MVC.Models
{
    public class OrderBook
    {
        [Key]
        public int? OrderID { get; set; }
        public int? BookID { get; set; }
        public int? Quantity { get; set; }
        public double? TSubPrice { get; set; }
        public Order? Order { get; set; }
        public Book? Book { get; set; }
    }
}
