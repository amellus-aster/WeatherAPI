using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using MyWeatherApplication.Application;
using MyWeatherApplication.Infrastructure;
var builder = WebApplication.CreateBuilder(args);
//add rate limit service
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = 5;
        opt.Window = TimeSpan.FromSeconds(10);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 0;
    });
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});
//add  services
builder.Services.AddControllers();
builder.Services.AddInfrastructureServices(builder.Configuration); 
builder.Services.AddApplicationServices(); 

//add openapi and swagger to explore the api 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();
var app = builder.Build();
//use middlewares
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();

}
//use UseHttpsRedirection to must use https
app.UseHttpsRedirection();
//use author mid
app.UseAuthorization();
//use rate
app.UseRateLimiter();
//map controller with retelimiting fixed
app.MapControllers().RequireRateLimiting("fixed");
app.Run();
