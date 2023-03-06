using Microsoft.EntityFrameworkCore;
using TypeRunnerBE.Models;
using TypeRunnerBE.Services.Auth;
using TypeRunnerBE.Services.Data;
using TypeRunnerBE.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<TypeRunnerContext>(opt =>
{
    var connectionString =
            new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build()
            .GetSection("ConnectionStrings")
            ["PostgreSQL"];

    opt.UseNpgsql(connectionString);
});
builder.Services
    .AddControllers()
    .AddNewtonsoftJson(setupAction => setupAction.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUsersService, EfcUsersService>();
builder.Services.AddScoped<ITokenBasedAuthService<string>, JwtAuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<TypeRunnerContext>();
    await DbInitializer.Initialize(context);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
