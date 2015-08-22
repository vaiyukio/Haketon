using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haketon.Models
{
    public class Registration 
    {
        public long Id { get; set; }
        public String Name { get; set; }
        public String KtpNumber { get; set; }
        public String PhoneNumber { get; set; }
        public String Address { get; set; }
        public bool IsVerified { get; set; }
    }
}
