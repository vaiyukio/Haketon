using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haketon.Models
{
    public class Outbox 
    {
        public long Id { get; set; }
        public String PhoneNumber { get; set; }
        public String Message { get; set; }
    }
}
