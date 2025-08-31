using DatabaseAccessLayer.Contexts;
using Microsoft.EntityFrameworkCore;
namespace BusinessLogicLayer.DesignPatterns.SingletonPattern
{
    public static class DbTool
    {
        private static MyContext _dbInstance;

        public static MyContext DbInstance
        {
            get
            {
                if (_dbInstance == null)
                {
                    var optionsBuilder = new DbContextOptionsBuilder<MyContext>();
                    optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=InvoiceDb;Trusted_Connection=True;TrustServerCertificate=True");

                    _dbInstance = new MyContext(optionsBuilder.Options);
                }

                return _dbInstance;
            }
        }
    }

}
