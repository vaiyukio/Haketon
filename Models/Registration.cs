using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Haketon.Models
{
    public class Registration : BaseEntity
    {
        public String IdentityNumber { get; set; }
        public String PhoneNumber { get; set; }
        public String Address { get; set; }
    }
}
