using FileWatcherService;

var builder = Host.CreateApplicationBuilder(args);

// TODO: implement configs for multiple clients
var clientConfigs = builder.Configuration.GetSection("Clients").Get<List<ClientConfig>>();

foreach(var clientConfig in clientConfigs)
{
    var serviceType = Type.GetType(clientConfig.ServiceType);

    if (serviceType != null)
    {
        builder.Services.AddSingleton(typeof(IFileWatcherService), serviceProvider =>
        {
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
    else
    {
        throw new Exception($"Service type '{clientConfig.ServiceType}' not found.");
    }
}

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
