using FileWatcherService;

var builder = Host.CreateApplicationBuilder(args);

// TODO: implement configs for multiple clients
var clientDirectory = @"C:\FTP";

builder.Services.AddSingleton<IFileWatcherService>(provider =>
    new FileWatcherService.FileWatcherService(clientDirectory, provider.GetRequiredService<ILogger<FileWatcherService.FileWatcherService>>()));

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
