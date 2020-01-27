using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Text;
using Z.EntityFramework.Extensions;

namespace BulkInsertDemo
{
    public class DemoContext : DbContext
    {
        public DemoContext(DbConnection connection) : base(connection, false)
        {
        }

        public DemoContext() : base()
        {
        }

        public DbSet<User> Users { get; set; }

        public void BulkInsert<TEntity>(IEnumerable<TEntity> entities, SqlBulkCopyOptions insertOptions = SqlBulkCopyOptions.Default | SqlBulkCopyOptions.CheckConstraints)
            where TEntity : class
        {
            var opt = new Action<EntityBulkOperation<TEntity>>(act =>
            {
                act.BatchSize = 5;
                act.BatchTimeout = Database.CommandTimeout ?? act.BatchTimeout;
                act.SqlBulkCopyOptions = (int)insertOptions;
                act.Transaction = Database.CurrentTransaction?.UnderlyingTransaction;
            });

            this.BulkInsert(entities, opt);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .Map(map =>
            {
                map.Properties(p => new
                {
                    p.Id,
                    p.Name,
                    p.Surname
                });
                map.ToTable("Users", "part");
            })
            // Map to the Users table  
            .Map(map =>
            {
                map.Properties(p => new
                {
                    p.Country,
                    p.City,
                    p.Address
                });
                map.ToTable("UserAddresses", "part");
            });
        }
    }
}
