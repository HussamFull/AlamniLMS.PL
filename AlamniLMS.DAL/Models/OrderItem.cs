using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.DAL.Models
{
    [PrimaryKey(nameof(OrderId), nameof(CourseId))]
    public class OrderItem
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public decimal totalPrice { get; set; }

        public int Count { get; set; }

        public decimal Price { get; set; }
    }
}
