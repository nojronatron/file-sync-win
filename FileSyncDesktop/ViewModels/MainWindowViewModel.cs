using System.Reflection;
using System.Windows;
using Caliburn.Micro;
using FileSyncDesktop.Helpers;
using FileSyncDesktop.Models;

namespace FileSyncDesktop.ViewModels
{
    public class MainWindowViewModel : Conductor<object>
    {
        private IFileWatcherSettings _fileWatcherSettings;
        private IRmzLogger _logger;

        public MainWindowViewModel(
            IFileWatcherSettings fileWatcherSettings, 
            IRmzLogger logger)
        {
            _logger = logger;
            _logger.Data("MainWindowViewModel:", "MainWindowViewModel created.");
            _fileWatcherSettings = fileWatcherSettings;
            _logger.Flush();
        }

        private string _fileSourcePath;
        public string FileSourcePath
        {
            get { return _fileSourcePath; }
            set
            {
                _fileSourcePath = value;
                NotifyOfPropertyChange(() => FileSourcePath);
            }
        }

        private string _filterArgument;
        public string FilterArgument
        {
            get { return _filterArgument; }
            set
            {
                _filterArgument = value;
                NotifyOfPropertyChange(() => FilterArgument);
            }
        }

        private string _serverAddress;
        public string ServerAddress
        {
            get { return _serverAddress; }
            set
            {
                _serverAddress = value;
                NotifyOfPropertyChange(() => ServerAddress);
            }
        }

        private int _serverPort;
        public int ServerPort
        {
            get { return _serverPort; }
            set
            {
                _serverPort = value;
                NotifyOfPropertyChange(() => ServerPort);
            }
        }

        private void NotifyConfigChanged()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            _logger.Data(methodName, "Notifying.");
            FileSourcePath = _fileWatcherSettings.FilePath;
            FilterArgument = _fileWatcherSettings.FileType;
            ServerAddress = _fileWatcherSettings.ServerAddress;

            if (int.TryParse(_fileWatcherSettings.ServerPort, out int srvrPort))
            {
                ServerPort = srvrPort;
            }
            else
            {
                ServerPort = -1;
            }

            _logger.Flush();
        }

        public bool CanClearConfiguration()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            _logger.Data(methodName, "Hard-coded TRUE.");
            _logger.Flush();
            return true;
        }

        public bool CanLoadConfiguration()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            _logger.Data(methodName, "Hard-coded TRUE.");
            _logger.Flush();
            return true;
        }

        public bool CanSetConfiguration()
        {
            // todo: implement if this becomes necessary
            string methodName = MethodBase.GetCurrentMethod().Name;
            _logger.Data(methodName, "Hard-coded FALSE.");
            _logger.Flush();
            return false;
        }

        public void ClearConfiguration()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            _logger.Data(methodName, "Called.");
            _fileWatcherSettings.RemoveFileSettings();
            _fileWatcherSettings.RemoveServerSettings();
            NotifyConfigChanged();
            _logger.Flush();
        }

        // set environment variables into the settings object
        public void LoadConfiguration()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            _logger.Data(methodName, "Called.");
            _logger.Flush();
            _fileWatcherSettings.GetSettingsFromEnvVars();
            NotifyConfigChanged();
            // use Conductor to launch chile ViewModels
            ActivateItem(IoC.Get<FileListViewModel>());
        }

        // set user-entered configuration items into the settings object
        public void SetConfiguration()
        {
            // todo: implement if this becomes necessary
            string methodName = MethodBase.GetCurrentMethod().Name;
            _logger.Data(methodName, "Called.");
            _logger.Data(methodName, "NO IMPLEMENTATION!");
            _logger.Flush();
        }

        public void MenuAbout()
        {
            string messageBoxTitle = "About File Sync Win";
            string messageBoxText = "File Sync Win\n\nVersion 0.0,1\n\nCreated by: Jon Rumsey\n\nhttps://github.com/nojronatron/file-sync-win";
            _logger.Data(messageBoxText, messageBoxTitle);
            MessageBox.Show(
                messageBoxText,
                messageBoxTitle,
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
            _logger.Flush();
        }

        public void MenuFileExit()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            _logger.Data(methodName, "Exiting MainWindowView.");
            _logger.Flush();
            _logger.Dispose();
            Application.Current.Shutdown();
        }

    }
}