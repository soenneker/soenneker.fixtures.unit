using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Soenneker.Utils.AutoBogus;
using Xunit;

namespace Soenneker.Fixtures.Unit.Abstract;

/// <summary>
/// A base xUnit fixture providing injectable log output, DI mechanisms like IServiceCollection and ServiceProvider, and AutoFaker/Faker for generating test data.
/// </summary>
public interface IUnitFixture : IAsyncLifetime
{
    ServiceProvider? ServiceProvider { get; set; }

    IServiceCollection Services { get; set; }

    Faker Faker { get; }

    AutoFaker AutoFaker { get; }
}