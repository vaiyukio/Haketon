using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haketon.Models
{
    public class OrderUser: Order
    {
        public string UserName  { get; set; }
        public double Longitude  { get; set; }
        public double Latitude  { get; set; }
        public string CommodityName  { get; set; }
    }
}
