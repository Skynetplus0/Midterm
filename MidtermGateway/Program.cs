using System.Threading.RateLimiting;

using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Reverse proxy
builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));



// Rate limiter
var permitLimit = builder.Configuration.GetValue<int>("RateLimiting:PermitLimit", 100);
var windowSeconds = builder.Configuration.GetValue<int>("RateLimiting:WindowSeconds", 60);
var queueLimit = builder.Configuration.GetValue<int>("RateLimiting:QueueLimit", 0);

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("gateway-policy", httpContext =>
    {
        var clientKey = httpContext.Request.Headers["X-Client-Id"].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(clientKey))
        {
            clientKey = httpContext.Connection.RemoteIpAddress?.ToString() ?? "anonymous";
        }

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: clientKey,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = permitLimit,
                Window = TimeSpan.FromSeconds(windowSeconds),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = queueLimit,
                AutoReplenishment = true
            });
    });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
   // app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        // Ana API'nin swagger.json dosyasýný Gateway üzerinden gösteriyoruz
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Main API (Via Gateway)");
        options.RoutePrefix = "swagger";
    });
}


app.UseHttpsRedirection();
app.UseRateLimiter();

app.MapGet("/", () => Results.Ok(new
{
    message = "StayBooking Gateway is running."
}));

app.MapReverseProxy().RequireRateLimiting("gateway-policy");

app.Run();