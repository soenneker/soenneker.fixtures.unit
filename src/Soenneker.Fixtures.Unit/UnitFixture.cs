using System;
using System.Threading.Tasks;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.XUnit.Injectable;
using Serilog.Sinks.XUnit.Injectable.Abstract;
using Serilog.Sinks.XUnit.Injectable.Extensions;
using Soenneker.Extensions.ValueTask;
using Soenneker.Fixtures.Unit.Abstract;
using Soenneker.Utils.AutoBogus;
using Soenneker.Utils.AutoBogus.Config;

namespace Soenneker.Fixtures.Unit;

///<inheritdoc cref="IUnitFixture"/>
public abstract class UnitFixture : IUnitFixture
{
    private readonly InjectableTestOutputSink? _sink;

    public ServiceProvider? ServiceProvider { get; set; }
    public IServiceCollection Services { get; set; }
    public Faker Faker { get; }
    public AutoFaker AutoFaker { get; }

    public UnitFixture(AutoFakerConfig? autoFakerConfig = null)
    {
        AutoFaker = new AutoFaker(autoFakerConfig);
        Faker = AutoFaker.Faker;

        Services = new ServiceCollection();
        Services = new ServiceCollection();

        _sink = new InjectableTestOutputSink();
        Services.AddSingleton<IInjectableTestOutputSink>(_sink);

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.InjectableTestOutput(_sink)
            .Enrich.FromLogContext()
            .CreateLogger();
    }

    public virtual ValueTask InitializeAsync()
    {
        ServiceProvider = Services.BuildServiceProvider();
        return ValueTask.CompletedTask;
    }

    public virtual async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);

        // Flush Serilog first
        await Log.CloseAndFlushAsync().NoSync();
        Log.Logger = Serilog.Core.Logger.None;

        // Because we provided the instance, DI won't dispose it — we must
        if (_sink != null)
            await _sink.DisposeAsync();

        if (ServiceProvider is not null)
            await ServiceProvider.DisposeAsync().NoSync();
    }
}