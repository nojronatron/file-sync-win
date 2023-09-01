using Caliburn.Micro;
using FileSyncDesktop.Collections;
using FileSyncDesktop.Helpers;
using FileSyncDesktop.Library.Api;
using FileSyncDesktop.Models;
using FileSyncDesktop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace FileSyncDesktop
{
    public class Bootstrapper : BootstrapperBase
    {
        private readonly SimpleContainer _container = new SimpleContainer();

        public Bootstrapper()
        {
            LogManager.GetLog = type => new DebugLog(type);
            Initialize();
        }

        protected override void Configure()
        {
            _container.Instance(_container)
                .PerRequest<IBibReportEndpoint, BibReportEndpoint>();

            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>()
                .Singleton<IRmzLogger, RmzLogger>()
                .Singleton<IFileWatcherSettings, FileWatcherSettings>()
                .Singleton<IFileDataProcessor, FileDataProcessor>()
                .Singleton<IBibRecordCollection, BibRecordCollection>()
                .Singleton<IFileListCollection, FileListCollection>()
                .Singleton<IAPIHelper, APIHelper>();

            foreach (var assembly in SelectAssemblies())
            {
                assembly.GetTypes()
                    .Where(type => type.IsClass)
                    .Where(type => type.Name.EndsWith("ViewModel"))
                    .ToList()
                    .ForEach(viewModelType => _container.RegisterPerRequest(
                        viewModelType, viewModelType.ToString(), viewModelType));
            }
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            var instance = _container.GetInstance(serviceType, key);
            if (instance != null)
            {
                return instance;
            }
            else
            {
                throw new InvalidOperationException("Could not locate any instances.");
            }
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return _container.GetAllInstances(serviceType);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<MainWindowViewModel>();
        }
        protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var logger = _container.GetInstance<IRmzLogger>();
            logger.Data("Bootstrapper.OnUnhandledException", "Called!");

            if (e.Exception != null)
            {
                logger.Data("Unhandled Exception", e.Exception.Message);
            } else
            {
                logger.Data("Bootstrapper.OnUnhandledException", "Exception is null.");
            }

            logger.Flush();
        }
    }
}
