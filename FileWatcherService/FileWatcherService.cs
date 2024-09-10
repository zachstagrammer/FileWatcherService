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

        public FileWatcherService(string directoryToWatch, ILogger<FileWatcherService> logger)
        {
            if (!Directory.Exists(directoryToWatch))
            {
                _logger.LogError($"Directory does not exist: {directoryToWatch}");
                throw new DirectoryNotFoundException($"Directory does not exist: {directoryToWatch}");
            }

            _fileWatcher = new FileSystemWatcher(directoryToWatch)
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName
            };

            _fileWatcher.Created += OnCreated;
            _fileWatcher.Changed += OnChanged;
            _fileWatcher.Deleted += OnDeleted;
            _fileWatcher.Error += OnError;

            _logger = logger;
        }

        public void StartWatching()
        {
            _fileWatcher.EnableRaisingEvents = true;
            _logger.LogInformation($"File watcher started: {_fileWatcher.GetType().Name}");
        }

        public void StopWatching()
        {
            _fileWatcher.EnableRaisingEvents= false;
            _fileWatcher.Dispose();
            _logger.LogInformation($"File watcher stopped: {_fileWatcher.GetType().Name}");
        }

        protected virtual void OnCreated(object sender, FileSystemEventArgs e)
        {
            _logger.LogInformation($"File created: {e.FullPath}");
        }

        protected virtual void OnChanged(object sender, FileSystemEventArgs e)
        {
            _logger.LogInformation($"File changed: {e.FullPath}");
        }

        protected virtual void OnDeleted(object sender, FileSystemEventArgs e)
        {
            _logger.LogInformation($"File deleted: {e.FullPath}");
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            _logger.LogError(e.GetException().Message);
        }
    }
}
