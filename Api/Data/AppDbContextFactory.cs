using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using Infrastructure.Data;

namespace Api.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<Infrastructure.Data.AppDbContext>
{
    public Infrastructure.Data.AppDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<Infrastructure.Data.AppDbContext>();
        var connectionString = config.GetConnectionString("DefaultConnection");

        optionsBuilder.UseSqlServer(connectionString);

        return new Infrastructure.Data.AppDbContext(optionsBuilder.Options);
    }
}
