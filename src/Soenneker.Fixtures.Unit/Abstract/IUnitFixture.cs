using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Soenneker.Utils.AutoBogus;
using Xunit;

namespace Soenneker.Fixtures.Unit.Abstract;

/// <summary>
/// Defines an xUnit fixture that owns a dependency-injection provider, injectable test logging, and shared fake-data generators.
/// </summary>
public interface IUnitFixture : IAsyncLifetime
{
    /// <summary>
    /// Gets or sets the service provider built during fixture initialization.
    /// </summary>
    ServiceProvider? ServiceProvider { get; set; }

    /// <summary>
    /// Gets or sets the services used to build <see cref="ServiceProvider"/> during fixture initialization.
    /// </summary>
    IServiceCollection Services { get; set; }

    /// <summary>
    /// Gets an instance of <see cref="Faker"/> used for generating fake data.
    /// </summary>
    Faker Faker { get; }

    /// <summary>
    /// Gets the <see cref="AutoFaker"/> used to generate populated object graphs.
    /// </summary>
    AutoFaker AutoFaker { get; }
}
