using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcherService
{
    public class FileWatcherService : IFileWatcherService
    {
        private readonly FileSystemWatcher _fileWatcher;
        private readonly ILogger<FileWatcherService> _logger;

        public FileWatcherService(string directoryToWatch, ILogger<FileWatcherService> logger)
        {
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
            _logger.LogInformation("File watcher started.");
        }

        public void StopWatching()
        {
            _fileWatcher.EnableRaisingEvents= false;
            _fileWatcher.Dispose();
            _logger.LogInformation("File watcher stopped.");
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
