using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace Application.IntegrationTests;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestsWebAppFactory>
{
    protected readonly IServiceScope _scope;
    protected readonly ApplicationDbContext _dbContext;

    protected BaseIntegrationTest(IntegrationTestsWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();
        _dbContext = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    }
}
