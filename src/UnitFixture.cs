using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.XUnit.Injectable;
using Serilog.Sinks.XUnit.Injectable.Abstract;
using Serilog.Sinks.XUnit.Injectable.Extensions;
using Xunit;

namespace Soenneker.Fixtures.Unit;

/// <summary>
/// A base xUnit fixture providing injectable log output and DI mechanisms like IServiceCollection and ServiceProvider
/// </summary>
public abstract class UnitFixture : IAsyncLifetime
{
    public ServiceProvider? ServiceProvider { get; set; }

    protected IServiceCollection Services { get; set; }

    public UnitFixture()
    {
        var injectableTestOutputSink = new InjectableTestOutputSink();

        // this needs to remain in constructor because of derivations
        Services = new ServiceCollection();

        Services.AddSingleton<IInjectableTestOutputSink>(injectableTestOutputSink);

        ILogger serilogLogger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Async(a => a.InjectableTestOutput(injectableTestOutputSink))
            .Enrich.FromLogContext()
            .CreateLogger();

        Log.Logger = serilogLogger;
    }

    public virtual Task InitializeAsync()
    {
        ServiceProvider = Services.BuildServiceProvider();

        return Task.CompletedTask;
    }
    
    public virtual async Task DisposeAsync()
    {
        GC.SuppressFinalize(this);

        if (ServiceProvider != null)
            await ServiceProvider.DisposeAsync();
    }
}