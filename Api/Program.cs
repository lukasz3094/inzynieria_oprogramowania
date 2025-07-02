using Api.Data;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Domain.Interfaces;
using Patterns.Facade;

namespace Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IMeetingRepository, MeetingRepository>();
		builder.Services.AddScoped<ICalendarRepository, CalendarRepository>();
		builder.Services.AddScoped<IMeetingFacade, MeetingFacade>();

        builder.Services.AddControllers();

        if (builder.Environment.IsEnvironment("Test")) { }
        else
        {
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        }

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

		if (!app.Environment.IsEnvironment("Test"))
        {
            int retries = 10;
            while (retries > 0)
            {
                try
                {
                    using var scope = app.Services.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    db.Database.Migrate();
                    Console.WriteLine("Database migration successful.");
                    break;
                }
                catch (Exception ex)
                {
                    retries--;
                    Console.WriteLine($"Api retries left: {retries}");
                    Console.WriteLine(ex.Message);
                    Thread.Sleep(5000);
                }
            }
        }

        app.MapControllers();
        app.Run();
    }
}
