using ECommerce.Domain.Entities.Concretes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.ViewModels
{
    public class OrderVM
    {
        public int Id { get; set; }
        public string? OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string? OrderNote { get; set; }
        public decimal Total { get; set; }
        public int CustomerId { get; set; }
        public ICollection<ProductVM> Products { get; set; } = new List<ProductVM>();
    }
}
