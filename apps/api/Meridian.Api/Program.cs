using Serilog;
using Serilog.Events;
using Serilog.Context;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Meridian.Api.Infrastructure.Data;



Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Use Serilog for the host (replaces default logging providers)
builder.Host.UseSerilog();

builder.Services.AddControllers();

// OpenAPI / Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Meridian Health Analytics API",
        Version = "v1",
        Description = "Enterprise-grade ingestion and analytics platform for healthcare data."
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", p =>
        p.WithOrigins("http://localhost:5173")
         .AllowAnyHeader()
         .AllowAnyMethod());
});


// API versioning (default v1.0 + reports supported versions)
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new(1, 0);
    options.ReportApiVersions = true;
});

// RFC 7807 error responses
builder.Services.AddProblemDetails();

// Health checks
var sqlConn = builder.Configuration.GetConnectionString("SqlServer")
    ?? throw new InvalidOperationException("Missing ConnectionStrings:SqlServer");

builder.Services.AddHealthChecks()
    .AddCheck("self", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy(),
        tags: new[] { "live" })
    .AddSqlServer(
        connectionString: sqlConn,
        name: "sqlserver",
        tags: new[] { "ready" });

builder.Services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler();
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Meridian API v1");
        c.RoutePrefix = "swagger";
    });
}

//app.UseHttpsRedirection();

// Correlation / Request Id (propagate or generate; echoed back to clients)
// IMPORTANT: Must run BEFORE request logging so logs include RequestId.
app.Use(async (context, next) =>
{
    const string headerName = "X-Request-Id";

    var requestId =
        context.Request.Headers.TryGetValue(headerName, out var value) && !string.IsNullOrWhiteSpace(value)
            ? value.ToString()
            : Guid.NewGuid().ToString("N");

    context.TraceIdentifier = requestId;
    context.Response.Headers[headerName] = requestId;

    using (LogContext.PushProperty("RequestId", requestId))
    {
        await next();
    }
});

// One structured log event per request (status code + elapsed time)
// Enriched with RequestId from TraceIdentifier (set above).
app.UseSerilogRequestLogging(options =>
{
    options.EnrichDiagnosticContext = (diag, httpContext) =>
    {
        diag.Set("RequestId", httpContext.TraceIdentifier);
    };
});

app.UseCors("AllowReact");


app.UseRouting();
app.UseAuthorization();

app.MapControllers();



app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = r => r.Tags.Contains("live")
});

app.MapHealthChecks("/ready", new HealthCheckOptions
{
    Predicate = r => r.Tags.Contains("ready")
});


try
{
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
