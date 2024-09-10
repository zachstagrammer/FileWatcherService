using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcherService
{
    public class ClientConfig
    {
        public string ClientName { get; set; }
        public string DirectoryToWatch { get; set; }
        public string ServiceType { get; set; }
    }
}
