using DatabaseAccessLayer.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

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
					var connectionString = Environment.GetEnvironmentVariable("DefaultConnection")
						?? "Server=(localdb)\\MSSQLLocalDB;Database=InvoiceDb;Trusted_Connection=True;TrustServerCertificate=True";

					optionsBuilder.UseSqlServer(connectionString, sql =>
					{
						sql.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null);
						sql.MigrationsAssembly(typeof(MyContext).Assembly.FullName);
					});
					optionsBuilder.CommandTimeout(60);

					_dbInstance = new MyContext(optionsBuilder.Options);
				}

				return _dbInstance;
			}
		}
	}

}
