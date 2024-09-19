using FileWatcherService;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

builder.Logging.AddSerilog(Log.Logger);

try
{
    Log.Information("Worker service starting up...");
    var clientConfigs = builder.Configuration.GetSection("Clients").Get<List<ClientConfig>>();

    if (clientConfigs == null)
    {
        throw new InvalidOperationException("No client configuration found.");
    }

    foreach (var clientConfig in clientConfigs)
    {
        var serviceType = Type.GetType(clientConfig.ServiceType);

        if (serviceType == null)
        {
            throw new TypeLoadException($"Service type '{clientConfig.ServiceType}' not found.");
        }

        builder.Services.AddSingleton(typeof(IFileWatcherService), serviceProvider =>
        {
            Log.Information("Registering file watcher service for directory {DirectoryToWatch}", clientConfig.DirectoryToWatch);
            var loggerType = typeof(ILogger<>).MakeGenericType(serviceType);
            var logger = serviceProvider.GetRequiredService(loggerType);
            return ActivatorUtilities.CreateInstance(
                serviceProvider,
                serviceType,
                clientConfig.DirectoryToWatch,
                logger
             );
        });
    }

    builder.Services.AddHostedService<Worker>();
    var host = builder.Build();
    host.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "The service failed to start correctly.");
}
finally
{
    Log.CloseAndFlush();
}




