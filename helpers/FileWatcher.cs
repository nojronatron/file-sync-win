using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Runtime.CompilerServices;
using file_sync_win.models;

namespace file_sync_win.helpers
{
    internal class FileWatcher : IDisposable
    {
        private bool disposedValue;

        private Logger Logger { get; set; }
        private FileWatcherSettings FileWatcherSettings { get; set;}
        private FileSystemWatcher FSWatcher { get; set; } = null;

        /// <summary>
        /// Constructor initializes a FileWatcher instance.
        /// Invalid or uninitialized parameters will throw an exception.
        /// </summary>
        /// <param name="logger">Logger Instance</param>
        public FileWatcher(Logger logger)
        {
            FileWatcherSettings = new FileWatcherSettings();
            Logger = logger;
        }

        public void Configure()
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Logger.Data(methodName, "Acquiring FileWatcher Settings.");
            FileWatcherSettings.GetSettings();
            FSWatcher = new FileSystemWatcher(FileWatcherSettings.FilePath, FileWatcherSettings.FileType);
            Logger.Data(methodName, FileWatcherSettings.ToString());
            Logger.Flush();
        }

        /// <summary>
        /// Returns an initialized FileSystemWatcher object.
        /// </summary>
        /// <returns></returns>
        public void Start()
        {
            var methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            if (FSWatcher == null)
            {
                Logger.Data(methodName, "FileSystemWatcher is not initialized (Call Configure first)");
            }
            else
            {
                try
                {
                    Logger.Data(methodName, "Starting FileSystemWatcher");
                    FSWatcher.Created += OnCreated;
                    FSWatcher.Error += OnError;
                    FSWatcher.IncludeSubdirectories = false;
                    FSWatcher.EnableRaisingEvents = true;
                }
                catch (Exception ex)
                {
                    Logger.Data(methodName, $"An exception occurred: {ex.Message}");
                }
            }
        }

        public void Stop()
        {
            var methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            if (FSWatcher == null)
            {
                Logger.Data(methodName, "FileSystemWatcher is not initialized");
            }
            else
            {
                try
                {
                    Logger.Data(methodName, "Stopping FileSystemWatcher");
                    FSWatcher.Created -= OnCreated;
                    FSWatcher.Error -= OnError;
                }
                catch (Exception ex)
                {
                    Logger.Data(methodName, $"An exception occurred: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// When a file is created, log the event to the common logging mechanism.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            string message = $"Created: {e.FullPath}";
            Console.WriteLine(message);
            Logger log = new Logger();
            log.Data("OnCreated:", message);
            log.Flush();
            log.Dispose();
        }

        private static void OnError(object sender, ErrorEventArgs e) => LogException(e.GetException());

        private static void LogException(Exception ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"Exception: {ex.Message}");  
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Logger = null;
                    FileWatcherSettings = null;
                    FSWatcher = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~FileWatcher()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
