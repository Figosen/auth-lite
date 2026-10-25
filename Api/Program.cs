using Api.BLL.Context;
using Api.Util;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddDbContext<AuthDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// IoC
builder.Services.AddScoped<IAuthLogic, AuthLogic>();

var jwtConfig = builder.Configuration.GetRequiredSection("Jwt");
var secretKey = builder.Configuration.GetValue<string>(Constants.Auth.Secretkey);
builder.Services.AddSingleton(new JwtBuilder(secretKey ?? ""));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// TODO: I'll maybe miss some CORS conf

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
