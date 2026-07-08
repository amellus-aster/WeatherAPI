using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using MyWeatherApplication.Application;
using MyWeatherApplication.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MyWeatherApplication.Infrastructure.Jwt;
using System.Text;
using Serilog;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

//clear logging provider 
builder.Logging.ClearProviders();
//Serilog Set up 
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();
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
//add authentication and jwt bearer services 
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        
        ValidIssuer = jwtSettings!.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
    };
});
//add  services
builder.Services.AddControllers();
builder.Services.AddInfrastructureServices(builder.Configuration); 
builder.Services.AddApplicationServices(); 

//add openapi and swagger to explore the api 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi(); 
var app = builder.Build();
//use middlewares
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); 
}
//use UseHttpsRedirection to must use https
app.UseHttpsRedirection();
//use IP logging middleware
app.UseMiddleware<IPLoggingMiddleware>();
//use author middleware
app.UseAuthentication(); 
app.UseAuthorization();
//use rate
app.UseRateLimiter();
//map controller with retelimiting fixed
app.MapControllers().RequireRateLimiting("fixed");
app.Run();
