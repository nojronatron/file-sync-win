using Caliburn.Micro;
using FileSyncDesktop.Collections;
using FileSyncDesktop.Helpers;
using FileSyncDesktop.Library.Api;
using FileSyncDesktop.Models;
using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Reflection;

namespace FileSyncDesktop.ViewModels
{
    public class FileListViewModel : Screen
    {
        private IRmzLogger _logger;
        private FileSystemWatcher _fsWatcher;
        private IFileDataProcessor _fileDataProcessor;
        private IFileWatcherSettings _fileWatcherSettings;
        private IBibReportEndpoint _bibReportEndpoint;

        private FileListCollection _fileList;
        public FileListCollection FileList
        {
            get { return _fileList; }
            set
            {
                _fileList = value;
                NotifyOfPropertyChange(() => FileListText);
            }
        }

        private string _fileListText;
        public string FileListText
        {
            get { return _fileListText; }
            set
            {
                _fileListText = value;
                NotifyOfPropertyChange(() => FileListText);
            }
        }

        private bool _fsWatcherIsRunning;
        public bool FsWatcherIsRunning
        {
            get { return _fsWatcherIsRunning; }
            set
            {
                _fsWatcherIsRunning = value;
                NotifyOfPropertyChange(() => FsWatcherIsRunning);
                NotifyOfPropertyChange(() => CanStartFSWatcher);
                NotifyOfPropertyChange(() => CanStopFSWatcher);
            }
        }

        public bool CanStartFSWatcher
        {
            get { return FsWatcherIsRunning ? false : true; }
        }

        public bool CanStopFSWatcher
        {
            get { return FsWatcherIsRunning ? true : false; }
        }

        [ImportingConstructor]
        public FileListViewModel(
            IBibReportEndpoint bibReportEndpoint,
            IFileDataProcessor fileDataProcessor,
            IFileWatcherSettings fileWatcherSettings,
            IRmzLogger logger)
        {
            _bibReportEndpoint = bibReportEndpoint;
            _logger = logger;
            _logger.Data("FileListViewModel", "Initializing.");
            _fileWatcherSettings = fileWatcherSettings;
            _fileDataProcessor = fileDataProcessor;
            _logger.Flush();
        }

        public bool ConfigureFSWatcher()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            _logger.Data(methodName, "Acquiring FileWatcher Settings.");
            _fileWatcherSettings.GetSettingsFromEnvVars();

            bool hasFilePathFilterSettings = HasFilePathFilterSettings();
            bool hasServerSettings = HasServerSettings();

            if (hasFilePathFilterSettings && hasServerSettings)
            {
                _logger.Data(methodName, "Client-side and Server-side variables set. Run as a Client-side instance.");
                _fsWatcher = new FileSystemWatcher(_fileWatcherSettings.FilePath, _fileWatcherSettings.FileType);
                _logger.Data(methodName, _fileWatcherSettings.ToString());
                _logger.Flush();
                return true;
            }

            if (hasFilePathFilterSettings && !hasServerSettings)
            {
                _logger.Data(methodName, "File path and filter settings found. Run as stand-alone watcher only.");
                _fsWatcher = new FileSystemWatcher(_fileWatcherSettings.FilePath, _fileWatcherSettings.FileType);
                _logger.Data(methodName, _fileWatcherSettings.ToString());
                _logger.Flush();
                return true;
            }

            _logger.Data(methodName, "Not enabling a server-settings only or no-settings configuration.");
            StopFSWatcher(); // also calls Dispose() on FSWatcher
            _logger.Flush();
            return false;
        }

        private bool HasServerSettings()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            if (_fileWatcherSettings.ServerAddress != string.Empty && _fileWatcherSettings.ServerPort != string.Empty)
            {
                _logger.Data(methodName, "Server address and port variables are set.");
                return true;
            }
            else
            {
                _logger.Data(methodName, "Server address and/or port variables are NOT set.");
                return false;
            }
        }

        private bool HasFilePathFilterSettings()
        {
            var methodName = MethodBase.GetCurrentMethod().Name;

            if (_fileWatcherSettings.FilePath != string.Empty && _fileWatcherSettings.FileType != string.Empty)
            {
                _logger.Data(methodName, "File path and file type settings found!");
                return true;
            }
            else
            {
                _logger.Data(methodName, "File path and/or file type settings MISSING!");
                return false;
            }
        }

        public void StartFSWatcher()
        {
            var methodName = MethodBase.GetCurrentMethod().Name;
            ConfigureFSWatcher();

            if (_fsWatcher == null)
            {
                _logger.Data(methodName, "FileSystemWatcher is not initialized (Call Configure first)");
                _logger.Flush();
                return;
            }
            else
            {
                try
                {
                    _logger.Data(methodName, "Starting FileSystemWatcher");
                    _fileList = new FileListCollection();
                    _fsWatcher.Created += OnCreated;
                    _fsWatcher.Error += OnError;
                    _fsWatcher.IncludeSubdirectories = false;
                    _fsWatcher.EnableRaisingEvents = true;
                    FsWatcherIsRunning = true;
                }
                catch (Exception ex)
                {
                    _logger.Data(methodName, $"An exception occurred: {ex.Message}");
                    FsWatcherIsRunning = false;
                }
            }

            _logger.Flush();
        }

        public void StopFSWatcher()
        {
            var methodName = MethodBase.GetCurrentMethod().Name;
            _logger.Data(methodName, "Called.");

            if (_fsWatcher == null)
            {
                _logger.Data(methodName, "FileSystemWatcher is not initialized");
            }
            else
            {
                try
                {
                    _logger.Data(methodName, "Stopping FileSystemWatcher");
                    _fsWatcher.EnableRaisingEvents = false;
                    _fsWatcher.Created -= OnCreated;
                    _fsWatcher.Error -= OnError;
                }
                catch (Exception ex)
                {
                    _logger.Data(methodName, $"An exception occurred: {ex.Message}");
                }
                finally
                {
                    _fsWatcher.Dispose();
                    _logger.Data(methodName, "FileSystemWatcher Disposed");
                }
            }

            FsWatcherIsRunning = false;
            _logger.Flush();
        }

        private async void OnCreated(object sender, FileSystemEventArgs e)
        {
            // sender is Syste.IO.FileSystemWatcher
            string fullPath = e.FullPath;
            string message = $"Created: {fullPath}";
            _logger.Data("OnCreated:", message);
            _fileList.Add(fullPath);
            _fileListText = _fileList.ToString();
            NotifyOfPropertyChange(() => FileListText);
            var fileProcessed = _fileDataProcessor.ProcessFile(fullPath);

            if (fileProcessed.bibRecords.Count < 1)
            {
                _logger.Data("OnCreated", "No records found in file. Not posting to server.");
                return;
            }

            var postBibRecordResult = await _bibReportEndpoint.PostBibReport(fileProcessed);
            string postResult = postBibRecordResult.Item1 ? "Posted records to server." : "Could not post records to a server.";
            string postResultMessage = postBibRecordResult.Item2;
            _logger.Data("OnCreated", postResult);
            _logger.Data("OnCreated", postResultMessage);
            _logger.Flush();
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            if (sender is FileListViewModel)
            {
                (sender as FileListViewModel).LogException(e.GetException());
            }
            else
            {
                // I'm not sure how this would happen, but if it does it will get logged
                _logger.Data("OnError - UNKNOWN CALLER", e.GetException().Message);
                _logger.Flush();
            }
        }

        public string GetFilePath()
        {
            return _fileWatcherSettings.FilePath;
        }

        public string GetFileType()
        {
            return _fileWatcherSettings.FileType;
        }

        public string GetServerAddress()
        {
            return _fileWatcherSettings.ServerAddress;
        }

        public string GetServerPort()
        {
            return _fileWatcherSettings.ServerPort;
        }

        private void LogException(Exception ex)
        {
            if (ex != null)
            {
                _logger.Data("LogException", $"Received exception {ex.Message}");
            }
        }
    }
}
