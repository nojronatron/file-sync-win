using System.Reflection;
using System.Windows;
using Caliburn.Micro;
using FileSyncDesktop.Helpers;
using FileSyncDesktop.Library.Helpers;

namespace FileSyncDesktop.ViewModels
{
    public class MainWindowViewModel : Conductor<object>
    {
        private static readonly string _defaultServerAddress = string.Empty;
        private static readonly string _defaultServerPort = string.Empty;
        private static readonly string _defaultFilepathArgument = string.Empty;
        private static readonly string _defaultFilterArgument = string.Empty;
        private readonly IFileWatcherSettings _fileWatcherSettings;
        private readonly IRmzLogger _logger;

        public MainWindowViewModel(
            IFileWatcherSettings fileWatcherSettings,
            IRmzLogger logger)
        {
            _logger = logger;
            _logger.Data("MainWindowViewModel", "MainWindowViewModel created.");
            _fileWatcherSettings = fileWatcherSettings;
            _logger.Flush();
        }

        private string _fileSourcePath = _defaultFilepathArgument;
        public string FileSourcePath
        {
            get { return _fileSourcePath; }
            set
            {
                _fileSourcePath = value;
                NotifyOfPropertyChange(() => FileSourcePath);
                NotifyOfPropertyChange(() => CanSetConfiguration);
            }
        }

        private string _filterArgument = _defaultFilterArgument;
        public string FilterArgument
        {
            get { return _filterArgument; }
            set
            {
                _filterArgument = value;
                NotifyOfPropertyChange(() => FilterArgument);
                NotifyOfPropertyChange(() => CanSetConfiguration);
            }
        }

        private string _serverAddress = _defaultServerAddress;
        public string ServerAddress
        {
            get { return _serverAddress; }
            set
            {
                _serverAddress = value;
                NotifyOfPropertyChange(() => ServerAddress);
                NotifyOfPropertyChange(() => CanSetConfiguration);
            }
        }

        private string _serverPort = _defaultServerPort;
        public string ServerPort
        {
            get
            {
                return _serverPort;
            }
            set
            {
                _serverPort = value;
                NotifyOfPropertyChange(() => ServerPort);
                NotifyOfPropertyChange(() => CanSetConfiguration);
            }
        }

        private void NotifyConfigChanged()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            _logger.Data(methodName, "Notifying.");
            _fileWatcherSettings.FilePath = FileSourcePath;
            _fileWatcherSettings.FileType = FilterArgument;
            _fileWatcherSettings.ServerAddress = ServerAddress;
            _fileWatcherSettings.ServerPort = ServerPort.ToString();
            LogCurrentConfig(methodName);
            _logger.Flush();
        }

        public bool CanClearConfiguration
        {
            get
            {
                return true;
            }
        }


        public bool CanLoadConfiguration
        {
            get
            {
                return string.IsNullOrEmpty(ServerPort) &&
                    string.IsNullOrEmpty(ServerAddress) &&
                    string.IsNullOrEmpty(FilterArgument) &&
                    string.IsNullOrEmpty(FileSourcePath);
            }
        }

        public bool CanSetConfiguration
        {
            get
            {
                return !string.IsNullOrEmpty(FileSourcePath) &&
                       !string.IsNullOrEmpty(FilterArgument) &&
                       !string.IsNullOrEmpty(ServerAddress) &&
                       !string.IsNullOrEmpty(ServerPort);
            }
        }

        public void ClearConfiguration()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            _logger.Data(methodName, "Clearing File Watcher settings.");
            _fileWatcherSettings.RemoveFileSettings();
            _fileWatcherSettings.RemoveServerSettings();
            _logger.Data(methodName, "Clearing form entries.");
            FileSourcePath = string.Empty;
            FilterArgument = string.Empty;
            ServerAddress = string.Empty;
            ServerPort = string.Empty;
            NotifyConfigChanged();
            NotifyOfPropertyChange(() => CanOpenFileMonitor);
            NotifyOfPropertyChange(() => CanLoadConfiguration);
            NotifyOfPropertyChange(() => CanSetConfiguration);
            _logger.Flush();
        }

        // set environment variables into the settings object
        public void LoadConfiguration()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            _logger.Data(methodName, "Called. Getting config settings from Environment Variables.");

            if (_fileWatcherSettings.GetSettingsFromEnvVars())
            {
                FileSourcePath = _fileWatcherSettings.FilePath;
                FilterArgument = _fileWatcherSettings.FileType;
                ServerAddress = _fileWatcherSettings.ServerAddress;
                ServerPort = _fileWatcherSettings.ServerPort;
                NotifyConfigChanged();
                LogCurrentConfig(methodName);
            }
            else
            {
                _logger.Data(methodName, "Unable to load Environment Variable settings.");
            }
            _logger.Flush();
        }

        // set user-entered configuration items into the settings object
        public void SetConfiguration()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            if (CanSetConfiguration)
            {
                _logger.Data(methodName, "Using arguments in form to set FileWatcher Configuration.");
                _fileWatcherSettings.SetFileSettings(FileSourcePath, FilterArgument);
                _fileWatcherSettings.SetServerSettings(ServerAddress, ServerPort);
                NotifyOfPropertyChange(() => CanOpenFileMonitor);
                NotifyOfPropertyChange(() => CanLoadConfiguration);
                LogCurrentConfig(methodName);
            }
            else
            {
                NotifyOfPropertyChange(() => CanOpenFileMonitor);
                NotifyOfPropertyChange(() => CanLoadConfiguration);
                _logger.Data(methodName, "Settings not valid, returning to Main View window.");
            }

            _logger.Flush();

        }

        private void LogCurrentConfig(string methodName)
        {
            _logger.Data(methodName, "Path, Filter, Server address and Port settings:");
            _logger.Data(methodName, FileSourcePath);
            _logger.Data(methodName, FilterArgument);
            _logger.Data(methodName, ServerAddress);
            _logger.Data(methodName, ServerPort.ToString());
        }

        public bool CanOpenFileMonitor
        {
            get
            {
                if (_fileWatcherSettings.HasFileSettings() && _fileWatcherSettings.HasServerSettings())
                {
                    return true;
                }

                return false;
            }
        }

        public void OpenFileMonitor()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            _logger.Data(methodName, "Called.");

            if (_fileWatcherSettings.HasFileSettings() && _fileWatcherSettings.HasServerSettings())
            {
                _logger.Data(methodName, "CanStartFileMonitor is True. Launching the File Watcher view.");
                _logger.Flush();
                // use Conductor to launch child ViewModel
                ActivateItem(IoC.Get<FileListViewModel>());
            }
            else
            {
                // todo: add status message indicating why file monitor can't be opened
                _logger.Data(methodName, "CanStartFileMonitor is False (Settings not valid). Returning to Main View window.");
                _logger.Flush();
            }
        }

        public void HelpAbout()
        {
            string messageBoxTitle = "About File Sync Win";
            string messageBoxText = "File Sync Win\n\nVersion 0.0.1\n\nCreated by: Jon Rumsey\n\nhttps://github.com/nojronatron/file-sync-win";
            _logger.Data(messageBoxText, messageBoxTitle);
            MessageBox.Show(
                messageBoxText,
                messageBoxTitle,
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
            _logger.Flush();
        }

        public void MinimizeWindow()
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        public void CloseApp()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            _logger.Data(methodName, "Exiting MainWindowView.");
            _logger.Flush();
            _logger.Dispose();
            Application.Current.Shutdown();
        }

    }
}