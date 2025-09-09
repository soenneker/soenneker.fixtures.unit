using System;
using System.Threading.Tasks;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.XUnit.Injectable;
using Serilog.Sinks.XUnit.Injectable.Abstract;
using Serilog.Sinks.XUnit.Injectable.Extensions;
using Soenneker.Fixtures.Unit.Abstract;
using Soenneker.Utils.AutoBogus;
using Soenneker.Utils.AutoBogus.Config;

namespace Soenneker.Fixtures.Unit;

///<inheritdoc cref="IUnitFixture"/>
public abstract class UnitFixture : IUnitFixture
{
    public ServiceProvider? ServiceProvider { get; set; }

    public IServiceCollection Services { get; set; }

    public Faker Faker { get; }

    public AutoFaker AutoFaker { get; }

    public UnitFixture(AutoFakerConfig? autoFakerConfig = null)
    {
        AutoFaker = new AutoFaker(autoFakerConfig);
        Faker = AutoFaker.Faker;

        // this needs to remain in constructor because of derivations
        Services = new ServiceCollection();

        var injectableTestOutputSink = new InjectableTestOutputSink();

        Services.AddSingleton<IInjectableTestOutputSink>(injectableTestOutputSink);

        ILogger serilogLogger = new LoggerConfiguration().MinimumLevel.Verbose()
                                                         .WriteTo.InjectableTestOutput(injectableTestOutputSink)
                                                         .Enrich.FromLogContext()
                                                         .CreateLogger();

        Log.Logger = serilogLogger;
    }

    public virtual ValueTask InitializeAsync()
    {
        ServiceProvider = Services.BuildServiceProvider();

        return ValueTask.CompletedTask;
    }

    public virtual async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);

        await Log.CloseAndFlushAsync().ConfigureAwait(false);
        Log.Logger = Serilog.Core.Logger.None;

        if (ServiceProvider != null)
            await ServiceProvider.DisposeAsync().ConfigureAwait(false);
    }
}