using Application;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WebApi.Extensions;
using WebApi.Seedings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();

builder.Services.AddScopedInterceptors();

builder.Services.AddDbContext(builder.Configuration);

builder.Services.AddScopedRepositories();

builder.Services.AddScopedServices();

builder.Services.AddAutoMappers();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationAssemblyMarker).Assembly));

builder.Services.AddJwtAuthentication();

builder.Services.AddCustomAuthorization();

builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "HR System API",
    });
});

var app = builder.Build();

if (!app.Environment.IsEnvironment("Testing"))
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<MainDbContext>();
        
        //dbContext.Database.EnsureDeleted();
        
        dbContext.Database.Migrate();
        RoleSeeding.SeedDatabase(dbContext);
        CompanySeeding.SeedDatabase(dbContext);
    }
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())    
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "HR System API");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// Необходимо для работы фабрики в WebApiTest
// Также нужно в WebApi.csproj добавить <PreserveCompilationContext>true</PreserveCompilationContext>
public partial class Program { }
