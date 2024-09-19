using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Logging;
using Moq;
using System.IO;

namespace FileWatcherService.Tests
{
    [TestClass]
    public class FileWatcherServiceTests
    {
        [TestMethod]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void StartWatching_DirectoryDoesNotExist_ThrowsDirectoryNotFoundException()
        {
            // Arrange
            var directoryToWatch = "C:/NonExistentDirectory";
            var mockLogger = new Mock<ILogger<FileWatcherService>>();
            var fileWatcherService = new FileWatcherService(directoryToWatch, mockLogger.Object);

            // Act
            fileWatcherService.StartWatching();

            // Assert is handled by the ExpectedException attribute
        }

        [TestMethod]
        public void StartWatching_ValidDirectory_LogsStartMessage()
        {
            // Arrange
            var directoryToWatch = Directory.GetCurrentDirectory();
            var mockLogger = new Mock<ILogger<FileWatcherService>>();
            var fileWatcherService = new FileWatcherService(directoryToWatch, mockLogger.Object);

            // Act
            fileWatcherService.StartWatching();

            // Assert: Verify that the logger was called with the correct information
            mockLogger.Verify(logger => logger.Log(
                It.Is<LogLevel>(level => level == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("File watcher started")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }
    }
}