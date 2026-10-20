var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddControllers();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// TODO: I'll maybe miss some CORS conf

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
