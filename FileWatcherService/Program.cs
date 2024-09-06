using FileWatcherService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

// TODO: implement configs for multiple clients

builder.Services.AddSingleton<IFileWatcherService>(provider =>
    new FileWatcherService.FileWatcherService("<path>", provider.GetRequiredService<ILogger<FileWatcherService.FileWatcherService>>()));

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
