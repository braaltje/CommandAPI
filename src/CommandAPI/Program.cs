using CommandAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace CommandAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        //register services here
        builder.Services.AddDbContext<CommandContext>(options => options.UseNpgsql
        (builder.Configuration.GetConnectionString("PostgreSqlConnection")));

        builder.Services.AddControllers();

        //register Irepo interface and its concrete class to DI service container
        builder.Services.AddScoped<ICommandAPIRepo, SqlCommandAPIRepo>();

        var app = builder.Build();
        //Request Pipeline; register middleware here
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        // app.MapGet("/", () => "Hello World!");

        app.Run();
    }
}
