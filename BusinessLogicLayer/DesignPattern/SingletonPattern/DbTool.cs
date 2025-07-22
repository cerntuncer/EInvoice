using DatabaseAccessLayer.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DesignPattern.SingletonPattern
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
