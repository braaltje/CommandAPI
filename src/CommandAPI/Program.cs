using CommandAPI.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Newtonsoft.Json.Serialization;

namespace CommandAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);                        

        var conStrBuilder = new NpgsqlConnectionStringBuilder(
            builder.Configuration.GetConnectionString("PostgreSqlConnection"));
        conStrBuilder.Password = builder.Configuration["Password"];
        var connection = conStrBuilder.ConnectionString;        

        //register services here
        // builder.Services.AddDbContext<CommandContext>(options => options.UseNpgsql(
        //     builder.Configuration.GetConnectionString("PostgreSqlConnection")));     
        builder.Services.AddDbContext<CommandContext>(options => options.UseNpgsql(connection));
        
        builder.Services.AddControllers().AddNewtonsoftJson(s => 
            s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());

        //AutoMapper
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        //register Irepo interface and its concrete class to DI service container
        builder.Services.AddScoped<ICommandAPIRepo, SqlCommandAPIRepo>();

        var app = builder.Build();
        //Request Pipeline; register middleware here
        app.UseRouting();
        app.UseEndpoints(endpoints => endpoints.MapControllers());

        // app.MapGet("/", () => "Hello World!");

        app.MapGet("/", () => connection);

        app.Run();
    }
}
