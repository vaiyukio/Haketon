using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;

namespace Haketon.Models
{
    public interface IDBContext
    {
        IDbSet<Test> Tests { get; set; }
        DbEntityEntry Entry(object entity);
        int SaveChanges();
    }
}
