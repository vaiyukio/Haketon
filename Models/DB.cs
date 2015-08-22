using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Pluralization;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace Haketon.Models
{
    public class DB : DbContext, IDBContext
    {
        public DB() { }
        public DB(string connectionString) : base(connectionString) { }

        public IDbSet<Registration> Registrations { get; set; }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Types()
            .Configure(c => c.ToTable(GetTableName(c.ClrType.Name), "public"));
            builder.Conventions.Add<CustomKeyConvention>();
        }
          public static string GetTableName(String typeName) 
        {
            var pluralizationService = (IPluralizationService) 
                DbConfiguration.DependencyResolver.GetService(typeof(IPluralizationService), "plural");

            var result = pluralizationService.Pluralize(typeName);

            result = Regex.Replace(result, ".[A-Z]", m => m.Value[0] + "_" + m.Value[1]);

            return result.ToLower(); 
        }
        public static string GetColumnName(String typeName) 
        {
            var result = typeName;

            result = Regex.Replace(result, ".[A-Z]", m => m.Value[0] + "_" + m.Value[1]);

            return result.ToLower(); 
        }

        public class CustomKeyConvention : Convention
        {
            public CustomKeyConvention()
            {
                Properties()
                    .Configure(config => config.HasColumnName(DB.GetColumnName(config.ClrPropertyInfo.Name)));
            }
        }
        
        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items) 
        {
            return base.ValidateEntity(entityEntry, items);
        }

        public override int SaveChanges()
        {
            foreach (var entity in ChangeTracker.Entries<BaseEntity>())
            {
                if (entity.State == EntityState.Added)
                    entity.Entity.DateCreated = DateTime.Now;
                else
                    entity.Property(p => p.DateCreated).IsModified = false;                    
                entity.Entity.DateModified = DateTime.Now;                    
            }            
            return base.SaveChanges();
        }
    }

    public static class DbContextExtensions
    {
        public static void Update<T>(this DbContext dbContext, T entity, params Expression<Func<T, object>>[] getUpdatedFields)
            where T: class
        {
            if (getUpdatedFields.Length == 0)
                return;
            dbContext.Set<T>().Attach(entity);
            foreach (var getUpdatedField in getUpdatedFields)
                dbContext.Entry<T>(entity).Property(getUpdatedField).IsModified = true;
            dbContext.SaveChanges();
        }
    }
    
}
