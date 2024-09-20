namespace FileWatcherService
{
    public interface IFileWatcherService
    {
        string DirectoryToWatch { get; }

        void StartWatching();
        void StopWatching();
        void RestartFileWatcher();
    }
}
