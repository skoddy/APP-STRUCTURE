﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPB.Data.Services
{
    public class SQLiteDb : DbContext, IDataSource
    {
        private string _connectionString = null;

        public SQLiteDb(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<OrderItem>().HasKey(e => new { e.OrderID, e.OrderLine });
        }

        public DbSet<DbVersion> DbVersion { get; set; }

        //public DbSet<Customer> Customers { get; set; }
        //public DbSet<Product> Products { get; set; }
        //public DbSet<Order> Orders { get; set; }
        //public DbSet<OrderItem> OrderItems { get; set; }

        //public DbSet<Category> Categories { get; set; }

        //public DbSet<CountryCode> CountryCodes { get; set; }
        //public DbSet<PaymentType> PaymentTypes { get; set; }
        //public DbSet<TaxType> TaxTypes { get; set; }
        //public DbSet<OrderStatus> OrderStatus { get; set; }
        //public DbSet<Shipper> Shippers { get; set; }
    }
}
