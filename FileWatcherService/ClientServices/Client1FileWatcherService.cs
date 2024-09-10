using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcherService.ClientServices
{
    internal class Client1FileWatcherService : FileWatcherService
    {
        public Client1FileWatcherService(string directoryToWatch, ILogger<FileWatcherService> logger) : base(directoryToWatch, logger)
        {

        }

        protected override void OnCreated(object sender, FileSystemEventArgs e)
        {
            // client1 specific logic
        }

        protected override void OnChanged(object sender, FileSystemEventArgs e)
        {
            // client1 specific logic
        }

        protected override void OnDeleted(object sender, FileSystemEventArgs e)
        {
            // client1 specific logic
        }
    }
}
