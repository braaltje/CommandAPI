namespace CommandAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        //register services here
        builder.Services.AddControllers();

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
