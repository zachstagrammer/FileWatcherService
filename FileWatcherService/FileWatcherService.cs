using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcherService
{
    public class FileWatcherService : IFileWatcherService
    {
        protected readonly FileSystemWatcher _fileWatcher;
        protected readonly ILogger<FileWatcherService> _logger;

        public string DirectoryToWatch { get; private set; }
        public bool IsWatching => _fileWatcher.EnableRaisingEvents;

        public FileWatcherService(string directoryToWatch, ILogger<FileWatcherService> logger)
        {
            _logger = logger;

            if (!Directory.Exists(directoryToWatch))
            {
                _logger.LogError("Directory does not exist: {DirectoryToWatch}", directoryToWatch);
                throw new DirectoryNotFoundException($"Directory does not exist: {directoryToWatch}");
            }

            DirectoryToWatch = directoryToWatch;
            _fileWatcher = new FileSystemWatcher(directoryToWatch)
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName
            };

            _fileWatcher.Created += OnCreated;
            _fileWatcher.Changed += OnChanged;
            _fileWatcher.Deleted += OnDeleted;
            _fileWatcher.Error += OnError;
        }



        public void StartWatching()
        {
            try
            {
                _fileWatcher.EnableRaisingEvents = true;
                _logger.LogInformation("File watcher started, monitoring directory: {Directory}", _fileWatcher.Path);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while starting the FileSystemWatcher");
                RestartFileWatcher();
            }
        }

        public void StopWatching()
        {
            _fileWatcher.EnableRaisingEvents = false;
            _fileWatcher.Dispose();
            _logger.LogInformation("File watcher stopped.");
        }

        public void RestartFileWatcher()
        {
            _fileWatcher.EnableRaisingEvents = false;
            Thread.Sleep(5000); // 5 second delay

            while (true)
            {
                try
                {
                    _logger.LogInformation("Attempting to restart FileSystemWatcher");
                    _fileWatcher.EnableRaisingEvents = true;
                    _logger.LogInformation("File watcher successfully started, monitoring directory: {Directory}", _fileWatcher.Path);
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to restart FileSystemWatcher, retrying in 2 minutes...");
                    Thread.Sleep(2 * 60 * 1000); // 2 minute delay
                }
            }
        }

        protected virtual void OnCreated(object sender, FileSystemEventArgs e)
        {
            _logger.LogInformation("File created: {FilePath}", e.FullPath);
        }

        protected virtual void OnChanged(object sender, FileSystemEventArgs e)
        {
            _logger.LogInformation("File changed: {FilePath}", e.FullPath);
        }

        protected virtual void OnDeleted(object sender, FileSystemEventArgs e)
        {
            _logger.LogInformation("File deleted: {FilePath}", e.FullPath);
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            _logger.LogError(e.GetException().Message);
            RestartFileWatcher();
        }
    }
}
