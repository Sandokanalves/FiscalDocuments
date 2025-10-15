using System.Net;
using FiscalDocuments.Api;
using FiscalDocuments.Infrastructure.Persistence.Context;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;

namespace FiscalDocuments.Tests.Integration.Api.Controllers;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            // Remover qualquer registro de DbContext que possa ter vindo do Program.cs
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Adicionar o DbContext para usar o banco de dados em memória SQLite
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite("DataSource=file:inmem?mode=memory&cache=shared");
            });

            // Construir o provedor de serviços para obter uma instância do DbContext
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<AppDbContext>();
                // Garantir que o banco de dados seja criado
                db.Database.EnsureCreated();
            }
        });
    }
}

public class FiscalDocumentsControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public FiscalDocumentsControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetDocumentById_Should_ReturnNotFound_WhenDocumentDoesNotExist()
    {
        // Act
        var response = await _client.GetAsync($"/api/fiscaldocuments/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}

