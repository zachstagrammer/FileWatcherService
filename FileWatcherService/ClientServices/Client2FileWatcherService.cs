using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcherService.ClientServices
{
    internal class Client2FileWatcherService : FileWatcherService
    {
        public Client2FileWatcherService(string directoryToWatch, ILogger<FileWatcherService> logger) : base(directoryToWatch, logger)
        {
            
        }
        
        /*
        protected override void OnCreated(object sender, FileSystemEventArgs e)
        {
            // client2 specific logic
        }

        protected override void OnChanged(object sender, FileSystemEventArgs e)
        {
            // client2 specific logic
        }

        protected override void OnRenamed(object sender, RenamedEventArgs e)
        {
            // client2 speficic logic
        }

        protected override void OnDeleted(object sender, FileSystemEventArgs e)
        {
            // client2 specific logic
        }
        */
    }
}
