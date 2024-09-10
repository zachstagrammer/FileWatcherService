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
            while (!stoppingToken.IsCancellationRequested)
            {
                foreach (var fileWatcherService in _fileWatcherServices)
                {
                    fileWatcherService.StartWatching();
                }
            }

            _logger.LogInformation("Worker stopping...");
            foreach (var fileWatcherService in _fileWatcherServices)
            {
                fileWatcherService.StopWatching();
            }
        }
    }
}
