using System.Reflection;
using Application;
using Domain.Companies;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using WebApi.Extensions;
    
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();

builder.Services.AddSqLiteContext(builder.Configuration);

builder.Services.AddScopedRepositories();

builder.Services.AddScopedServices();

builder.Services.AddAutoMappers();

//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationAssemblyMarker).Assembly));

builder.Services.AddJwtAuthentication();

builder.Services.AddAuthorization();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MainDbContext>();

    //test
    dbContext.Database.EnsureDeleted();
    
    dbContext.Database.Migrate();
    
    //test
    if (!dbContext.Companies.Any())
    {
        dbContext.Companies.Add(Company.Create("Test"));
        dbContext.SaveChanges();
    }
    if (!dbContext.Roles.Any())
    {
        dbContext.Roles.Add(Role.Create("Admin"));
        dbContext.SaveChanges();
    }
    if (!dbContext.Users.Any())
    {
        dbContext.Users.Add(User.Create(dbContext.Roles.First().Id, dbContext.Companies.First().Id, "root", "Root000!"));
        dbContext.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();