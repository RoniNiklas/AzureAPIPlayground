using Microsoft.ApplicationInsights.Extensibility;
using Serilog;

// Bootstrap logger
Log.Logger = new LoggerConfiguration()
     .WriteTo.Console()
     .CreateBootstrapLogger();

try {
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Host.UseSerilog((context, services, loggerConfiguration) =>
        loggerConfiguration
            .WriteTo
            .ApplicationInsights(TelemetryConfiguration.CreateDefault(),
                TelemetryConverter.Traces));

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment()) {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex) {
    Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
}
finally {
    Log.CloseAndFlush();
}
