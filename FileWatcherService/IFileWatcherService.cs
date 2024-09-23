namespace FileWatcherService
{
    public interface IFileWatcherService
    {
        string DirectoryToWatch { get; }
        bool IsWatching { get; }

        void StartWatching();
        void StopWatching();
        void RestartFileWatcher();
        void Dispose();
    }
}
