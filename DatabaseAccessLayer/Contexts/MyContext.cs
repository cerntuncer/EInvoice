using DatabaseAccessLayer.Entities;
using DatabaseAccessLayer.Options;
using MAP.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;



namespace DatabaseAccessLayer.Contexts
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {
        }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Case> Cases { get; set; }
        public DbSet<Current> Currents { get; set; }
        public DbSet<CustomerSupplier> CustomerSuppliers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<LineOfInvoice> LineOfInvoices { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<ProductAndService> ProductsAndServices { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserCredential> UserCredentials { get; set; }


       
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            //birden fazla veri gönderildiğin herhangi biri hata alırsa bu istekle oluşanları kaydetmiyor 
            return await this.Database.BeginTransactionAsync();
        }
    }
}