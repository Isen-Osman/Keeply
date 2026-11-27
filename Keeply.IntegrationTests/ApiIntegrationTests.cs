using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Keeply.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace Keeply.IntegrationTests;

public class ApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ApiIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Products_RequiresAuth()
    {
        var response = await _client.GetAsync("/api/products");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task FullFlow_Works()
    {
        var email = $"user{Guid.NewGuid():N}@test.com";
        var register = await _client.PostAsJsonAsync("/api/auth/register", new { email, password = "Password123!" });
        var body = await register.Content.ReadAsStringAsync();
        Assert.True(register.IsSuccessStatusCode, body);
        var auth = await register.Content.ReadFromJsonAsync<AuthResponse>();
        Assert.True(!string.IsNullOrWhiteSpace(auth?.Token), body);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth!.Token);

        var create = await _client.PostAsJsonAsync("/api/products", new { name = "iPhone", store = "Apple", price = 1000m, purchaseDate = "2026-01-01", warrantyMonths = 12 });
        var createBody = await create.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.Created, create.StatusCode);
        var product = await create.Content.ReadFromJsonAsync<ProductResponse>();

        var list = await _client.GetFromJsonAsync<List<ProductResponse>>("/api/products");
        Assert.Single(list!);

        var claim = await _client.PostAsJsonAsync("/api/claims", new { productId = product!.Id, description = "Broken screen" });
        claim.EnsureSuccessStatusCode();

        var summary = await _client.GetFromJsonAsync<DashboardSummary>("/api/dashboard/summary");
        Assert.Equal(1, summary!.TotalProducts);
        Assert.Equal(1, summary.OpenClaims);
    }

    private record AuthResponse(string Token, string RefreshToken);
    private record ProductResponse(Guid Id, Guid UserId, string Name, string Store, decimal Price, DateOnly PurchaseDate, int WarrantyMonths, DateOnly WarrantyExpirationDate);
    private record DashboardSummary(int TotalProducts, int ActiveWarranties, int ExpiringWarranties, int ExpiredWarranties, int OpenClaims, int TotalDocuments);
}

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "integration-test-super-secret-key-1234567890",
                ["Jwt:Issuer"] = "Keeply",
                ["Jwt:Audience"] = "Keeply"
            });
        });
        builder.ConfigureServices(services =>
        {
            var dbName = $"KeeplyTests-{Guid.NewGuid()}";
            foreach (var d in services.Where(d => d.ServiceType == typeof(KeeplyDbContext) || d.ServiceType.ToString().Contains("DbContextOptions")).ToList())
                services.Remove(d);
            services.AddDbContext<KeeplyDbContext>(options => options.UseInMemoryDatabase(dbName));

            services.PostConfigureAll<JwtBearerOptions>(options =>
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("integration-test-super-secret-key-1234567890"));
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "Keeply",
                    ValidAudience = "Keeply",
                    IssuerSigningKey = key
                };
            });
        });
    }
}
