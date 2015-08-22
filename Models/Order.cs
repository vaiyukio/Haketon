using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haketon.Models
{
    public class Order
    {
        public long Id { get; set; }
        public long fkUserId { get; set; }
        public long CommodityType { get; set; }
        public long Amount { get; set; }
        public long Price { get; set; }
        public String OrderType { get; set; }
        public DateTime Date { get; set; }
        public long? fkMatchingOrderId { get; set; }
        public bool IsMatchSearched { get; set; }
    }
}
