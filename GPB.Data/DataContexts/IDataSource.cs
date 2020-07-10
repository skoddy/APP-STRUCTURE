using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GPB.Data
{
    public interface IDataSource : IDisposable
    {
        DbSet<DbVersion> DbVersion { get; }

        //DbSet<Category> Categories { get; }
        //DbSet<CountryCode> CountryCodes { get; }
        //DbSet<OrderStatus> OrderStatus { get; }
        //DbSet<PaymentType> PaymentTypes { get; }
        //DbSet<Shipper> Shippers { get; }
        //DbSet<TaxType> TaxTypes { get; }

        //DbSet<Customer> Customers { get; }
        //DbSet<Order> Orders { get; }
        //DbSet<OrderItem> OrderItems { get; }
        //DbSet<Product> Products { get; }

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
