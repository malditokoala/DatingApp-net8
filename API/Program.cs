using API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP Request pipeline
// El orden importa de como ese ponen los middlewares de abajo
app.UseCors(x => x.AllowAnyMethod().AllowAnyHeader().WithOrigins("http://localhost:4200", "https://localhost:4200"));
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
