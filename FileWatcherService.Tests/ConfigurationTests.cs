using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcherService.Tests
{
    [TestClass]
    public class ConfigurationTests
    {
        [TestMethod]
        public void CanReadClientConfigFromConfiguration()
        {
            // Arrange
            var inMemorySettings = new Dictionary<string, string>
            {
                {"Clients:0:ClientName", "Client1"},
                {"Clients:0:DirectoryToWatch", "C:/FTP/Client1"},
                {"Clients:0:ServiceType", "YourNamespace.Client1FileWatcherService"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var clientConfig = configuration.GetSection("Clients:0").Get<ClientConfig>();

            // Assert
            Assert.AreEqual("Client1", clientConfig.ClientName);
            Assert.AreEqual("C:/FTP/Client1", clientConfig.DirectoryToWatch);
            Assert.AreEqual("YourNamespace.Client1FileWatcherService", clientConfig.ServiceType);
        }
    }
}
