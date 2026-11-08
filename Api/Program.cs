using System.Text;
using Api.BLL.Context;
using Api.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, ct) =>
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
        document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        };
        return Task.CompletedTask;
    });
});

builder.Services.AddControllers();
builder.Services.AddDbContext<AuthDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// IoC
builder.Services.AddScoped<IAuthLogic, AuthLogic>();


var jwtConfig = builder.Configuration.GetRequiredSection("Jwt");
var secretKey = builder.Configuration.GetValue<string>(Constants.Auth.Secretkey);
builder.Services.AddSingleton(new JwtBuilder(secretKey ?? ""));

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme) // TODO. Midlertidigt - lav egen middleware senere
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["SecretKey"]!)), //TODO: væk med magic string

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,

            ValidateIssuer = true,
            ValidIssuer = jwtConfig["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtConfig["Audience"]
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// TODO: I'll maybe miss some CORS conf
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
