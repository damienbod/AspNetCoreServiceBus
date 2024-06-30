
using AspNetCoreServiceBusApi2.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using ServiceBusMessaging;

namespace AspNetCoreServiceBusApi2;

internal static class StartupExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;

        services.AddControllers();

        var connection = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<PayloadContext>(options =>
            options.UseSqlite(connection));

        services.AddSingleton<IServiceBusConsumer, ServiceBusConsumer>();
        services.AddSingleton<IServiceBusTopicSubscription, ServiceBusTopicSubscription>();
        services.AddSingleton<IProcessData, ProcessData>();

        services.AddHostedService<WorkerServiceBus>();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Payload API",
            });
        });

        return builder.Build();
    }

    public static WebApplication Configure(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHsts();
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