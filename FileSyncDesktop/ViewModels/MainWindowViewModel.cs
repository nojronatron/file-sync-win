using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Navigation;
using Caliburn.Micro;
using FileSyncDesktop.Helpers;
using FileSyncDesktop.Models;

namespace FileSyncDesktop.ViewModels
{
    public class MainWindowViewModel : Conductor<object>
    {
        private static readonly string _defaultServerAddress = "localhost";
        private static readonly int _defaultServerPort = 8001;
        private static readonly int _minServerPort = 8001;
        private static readonly int _maxServerPort = 65536;
        private static readonly string _defaultFilepathArgument = @"C:\Users\Public\Documents\";
        private static readonly Regex _filepathRegex = new Regex(@"^\w:\\((\S)*\\{0,1})*$");
        private static readonly string _defaultFilterArgument = "*.mime";
        private static readonly Regex _filterRegex = new Regex(@"^\*\.\w{0,4}$");

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
                var matches = _filepathRegex.Matches(value);

                if (matches.Count > 0)
                {
                    _fileSourcePath = value;
                }
                else
                {
                    _fileSourcePath = _defaultFilepathArgument;
                }

                NotifyOfPropertyChange(() => FileSourcePath);
            }
        }

        private string _filterArgument = _defaultFilterArgument;
        public string FilterArgument
        {
            get { return _filterArgument; }
            set
            {
                var matches = _filterRegex.Matches(value);

                if (matches.Count > 0)
                {
                    _filterArgument = value;
                }
                else
                {
                    _filterArgument = _defaultFilterArgument;
                }

                NotifyOfPropertyChange(() => FilterArgument);
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
            }
        }

        private int _serverPort = _defaultServerPort;
        public int ServerPort
        {
            get
            {
                    return _serverPort;
            }
            set
            {
                if (value >= _minServerPort && value < _maxServerPort)
                {
                    _serverPort = value;
                }
                else
                {
                    _serverPort = 7001;
                }

                NotifyOfPropertyChange(() => ServerPort);
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
            _logger.Data(methodName, "Path, Filter, Server address and Port settings:");
            _logger.Data(methodName, FileSourcePath);
            _logger.Data(methodName, FilterArgument);
            _logger.Data(methodName, ServerAddress);
            _logger.Data(methodName, ServerPort.ToString());
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
                return true;
            }
        }

        public bool CanSetConfiguration
        {
            get
            {
                return !string.IsNullOrWhiteSpace(FileSourcePath) &&
                    !string.IsNullOrWhiteSpace(FilterArgument) &&
                    !string.IsNullOrWhiteSpace(ServerAddress) &&
                    (ServerPort >= _minServerPort && ServerPort < _maxServerPort);
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
            ServerPort = _defaultServerPort;
            NotifyConfigChanged();
            _logger.Flush();
        }

        // set environment variables into the settings object
        public void LoadConfiguration()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            _logger.Data(methodName, "Called. Getting config settings from Environment Variables.");
            _fileWatcherSettings.GetSettingsFromEnvVars();
            _logger.Flush();
            NotifyConfigChanged();
        }

        // set user-entered configuration items into the settings object
        public void SetConfiguration()
        {
            // todo: implement if this becomes necessary
            string methodName = MethodBase.GetCurrentMethod().Name;
            _logger.Data(methodName, "Using arguments in form to set FileWatcher Configuration.");
            _fileWatcherSettings.SetFileSettings(FileSourcePath, FilterArgument);
            _fileWatcherSettings.SetServerSettings(ServerAddress, ServerPort);
            _logger.Data(methodName, "Launching the File Watcher view.");
            _logger.Flush();

            // use Conductor to launch child ViewModel
            ActivateItem(IoC.Get<FileListViewModel>());
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