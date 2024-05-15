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
    /// <summary>
    /// Gets or sets the service provider used to resolve dependencies.
    /// </summary>
    ServiceProvider? ServiceProvider { get; set; }

    /// <summary>
    /// Gets or sets the collection of service descriptors.
    /// </summary>
    IServiceCollection Services { get; set; }

    /// <summary>
    /// Gets an instance of <see cref="Faker"/> used for generating fake data.
    /// </summary>
    Faker Faker { get; }

    /// <summary>
    /// Gets an instance of <see cref="AutoFaker"/> used for generating auto-mocked fake data.
    /// </summary>
    AutoFaker AutoFaker { get; }
}