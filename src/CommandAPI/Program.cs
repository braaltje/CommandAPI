using CommandAPI.Data;

namespace CommandAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        //register services here
        builder.Services.AddControllers();

        //register Irepo interface to DI service container
        builder.Services.AddScoped<ICommandAPIRepo, MockCommandAPIRepo>();

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
