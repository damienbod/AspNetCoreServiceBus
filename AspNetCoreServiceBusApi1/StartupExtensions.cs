using Microsoft.OpenApi.Models;
using Serilog;
using ServiceBusMessaging;
namespace AspNetCoreServiceBusApi1;

internal static class StartupExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;

        services.AddControllers();

        services.AddScoped<ServiceBusSender>();
        services.AddScoped<ServiceBusTopicSender>();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Payload View API",
            });
        });

        return builder.Build();
    }

    public static WebApplication Configure(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        if (app.Environment!.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseStatusCodePagesWithReExecute("~/error");
        }

        app.UseStaticFiles();
        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();
        app.UseCors();

        app.MapControllers();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Payload Management API V1");
        });

        return app;
    }
}