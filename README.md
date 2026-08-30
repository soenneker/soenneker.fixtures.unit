[![](https://img.shields.io/nuget/v/Soenneker.Fixtures.Unit.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Fixtures.Unit/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.fixtures.unit/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.fixtures.unit/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/Soenneker.Fixtures.Unit.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Fixtures.Unit/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.fixtures.unit/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.fixtures.unit/actions/workflows/codeql.yml)

# Soenneker.Fixtures.Unit

An extensible xUnit fixture that creates a dependency-injection container, configures injectable Serilog test output, and provides shared Bogus generators.

## Install

```bash
dotnet add package Soenneker.Fixtures.Unit
```

## Usage

Create a fixture and register its services in the constructor. xUnit calls `InitializeAsync` after construction, which builds the provider from those registrations.

```csharp
using Microsoft.Extensions.DependencyInjection;
using Soenneker.Fixtures.Unit;

public sealed class TestFixture : UnitFixture
{
    public TestFixture()
    {
        Services.AddSingleton<IClock, TestClock>();
        Services.AddTransient<OrderService>();
    }
}
```

Share the fixture through an xUnit collection:

```csharp
using Microsoft.Extensions.DependencyInjection;
using Xunit;

[CollectionDefinition("unit")]
public sealed class UnitCollection : ICollectionFixture<TestFixture>;

[Collection("unit")]
public sealed class OrderServiceTests
{
    private readonly TestFixture _fixture;

    public OrderServiceTests(TestFixture fixture) => _fixture = fixture;

    [Fact]
    public void Creates_an_order()
    {
        OrderService service = _fixture.ServiceProvider!
                                               .GetRequiredService<OrderService>();

        string customerName = _fixture.Faker.Name.FullName();
        Order request = _fixture.AutoFaker.Generate<Order>();

        // exercise service
    }
}
```

The fixture also registers its `IInjectableTestOutputSink`. Inject the current `ITestOutputHelper` into that singleton when a test class is constructed if logs should appear in that test's output:

```csharp
using Serilog.Sinks.XUnit.Injectable.Abstract;
using Xunit;

IInjectableTestOutputSink sink = fixture.ServiceProvider!
                                            .GetRequiredService<IInjectableTestOutputSink>();
sink.Inject(testOutputHelper);
```

## Lifecycle and behavior

- Add or replace registrations before `InitializeAsync` runs. Changing `Services` after the provider is built does not update `ServiceProvider`.
- `Faker` is the Bogus generator owned by `AutoFaker`; use either property depending on whether a value or an entire object graph is needed.
- The fixture owns the provider and injectable sink and disposes both during xUnit fixture teardown. Consumers should still dispose any scopes they create.
- Construction assigns the process-wide `Serilog.Log.Logger`, and teardown flushes it. Use one shared fixture for a test collection or assembly; multiple fixture instances running in parallel can replace each other's global logger and route output to the wrong test.
