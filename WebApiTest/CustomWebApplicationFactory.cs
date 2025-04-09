using WebApi.Seedings;

namespace WebApiTest;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.DbContexts;
using Microsoft.Data.Sqlite;
using System.Data.Common;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private DbConnection _connection = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        builder.ConfigureServices(services =>
        {
            // --- Настройка Базы Данных ---

            // Удаляем оригинальную регистрацию DbContextOptions
            var dbContextOptionsDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<MainDbContext>));
            if (dbContextOptionsDescriptor != null)
            {
                services.Remove(dbContextOptionsDescriptor);
            }

            // Удаляем оригинальную регистрацию DbContext, если она есть
            var dbContextDescriptor = services.SingleOrDefault(
                 d => d.ServiceType == typeof(MainDbContext));
            if (dbContextDescriptor != null)
            {
                services.Remove(dbContextDescriptor);
            }

            // Регистрируем DbContext с использованием открытого соединения SQLite In-Memory
            services.AddDbContext<MainDbContext>(options =>
            {
                options.UseSqlite(_connection); // Используем существующее открытое соединение
            });
            
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<MainDbContext>();
                db.Database.EnsureCreated();
                RoleSeeding.SeedDatabase(db);
                CompanySeeding.SeedDatabase(db);
                VacancySeeding.SeedDatabase(db);
                CandidateSeeding.SeedDatabase(db);
            }
        });
        builder.UseEnvironment("Testing");
    }
}