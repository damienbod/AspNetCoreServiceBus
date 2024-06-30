using AspNetCoreServiceBusApi1;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up OpeniddictServer");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, loggerConfiguration) => loggerConfiguration
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
        .ReadFrom.Configuration(context.Configuration));

    var app = builder
        .ConfigureServices()
        .Configure();

    app.Run();
}
catch (Exception ex) when (ex.GetType().Name is not "StopTheHostException" && ex.GetType().Name is not "HostAbortedException")
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}