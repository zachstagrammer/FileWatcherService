namespace FileWatcherService
{
    public interface IFileWatcherService
    {
        void StartWatching();
        void StopWatching();
        void RestartFileWatcher();
    }
}
