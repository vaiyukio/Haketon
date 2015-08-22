using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Haketon.Models
{
    public class Test : BaseEntity
    {
        public string Name { get; set; }
    }
}
