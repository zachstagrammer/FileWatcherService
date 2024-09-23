namespace FileWatcherService
{
    public class Worker : BackgroundService
    {
        private readonly IEnumerable<IFileWatcherService> _fileWatcherServices;
        private readonly ILogger<Worker> _logger;

        public Worker(IEnumerable<IFileWatcherService> fileWatcherServices, ILogger<Worker> logger)
        {
            _fileWatcherServices = fileWatcherServices;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker starting...");

            foreach (var fileWatcherService in _fileWatcherServices)
            {
                fileWatcherService.StartWatching();
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                foreach (var fileWatcherService in _fileWatcherServices)
                {
                    await MonitorDirectoryHealth(fileWatcherService, stoppingToken);
                }

                await Task.Delay(30 * 1000, stoppingToken); // 30 second delay between health checks
            }

            _logger.LogInformation("Worker stopping...");

            foreach (var fileWatcherService in _fileWatcherServices)
            {
                fileWatcherService.StopWatching();
                fileWatcherService.Dispose();
            }
        }

        private async Task MonitorDirectoryHealth(IFileWatcherService fileWatcherService, CancellationToken stoppingToken)
        {
            if (!Directory.Exists(fileWatcherService.DirectoryToWatch) && fileWatcherService.IsWatching)
            {
                _logger.LogWarning("Directory {DirectoryToWatch} is unavailable.", fileWatcherService.DirectoryToWatch);
                fileWatcherService.StopWatching();
                await Task.Delay(5 * 1000, stoppingToken);
            }

            if (Directory.Exists(fileWatcherService.DirectoryToWatch) && !fileWatcherService.IsWatching)
            {
                _logger.LogInformation("Directory {DirectoryToWatch} is now available. Restarting watcher...", fileWatcherService.DirectoryToWatch);
                fileWatcherService.StartWatching();
            }
        }

    }
}
