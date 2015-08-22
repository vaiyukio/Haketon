using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haketon.Models
{
    public class User
    {
        public long Id { get; set; }
        public String Name { get; set; }
        public String KtpNumber { get; set; }
        public String PhoneNumber { get; set; }
        public String Address { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
