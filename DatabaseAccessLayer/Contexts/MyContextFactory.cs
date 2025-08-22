using DatabaseAccessLayer.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DatabaseAccessLayer.Contexts
{
	public class MyContextFactory : IDesignTimeDbContextFactory<MyContext>
	{
		public MyContext CreateDbContext(string[] args)
		{
			var configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: true)
				.AddJsonFile("appsettings.Development.json", optional: true)
				.AddEnvironmentVariables()
				.Build();

			var connectionString = configuration.GetConnectionString("DefaultConnection")
				?? "Server=(localdb)\\MSSQLLocalDB;Database=InvoiceDb;Trusted_Connection=True;TrustServerCertificate=True";

			var optionsBuilder = new DbContextOptionsBuilder<MyContext>();
			optionsBuilder.UseSqlServer(connectionString, sql =>
			{
				sql.MigrationsAssembly(typeof(MyContext).Assembly.FullName);
			});

			return new MyContext(optionsBuilder.Options);
		}
	}
}