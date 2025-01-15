using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using PdfTools.Commands;
using PdfTools.Decorator;
using PdfTools.Logging.Contracts;
using PdfTools.Logging.NLog;
using PdfTools.PdfServices;

namespace PdfTools.ServiceLocator
{
    /// <summary>
    ///  Lifecycle Management
    /// </summary>
    public static class PtDiServiceLocator
    {
        private static readonly ServiceProvider _container;

        static PtDiServiceLocator()
        {
            var builder = new ServiceCollection();
            ConfigureServices(builder);
            _container = builder.BuildServiceProvider();
        }

        private static void ConfigureServices(ServiceCollection builder)
        {
            builder.AddTransient<IPtServiceProvider, PtServiceProviderAdapter>();

            var lf = new PtNlogLoggerFactory();
            builder.AddSingleton<IPtLoggerFactory>(lf);
            builder.AddSingleton<IPtLogger>(lf.CreateLogger());

            builder.AddTransient<IHttpClient, HttpClientFacade>();
            builder.AddTransient<PdfArchiver, PdfArchiver>();
            // web reader ist ZIIP

            //builder.AddTransient<ICommand, BatchCommand>();
            builder.AddTransient<ICommand>(provider =>
            {
                return new BatchCommand("daac", provider.GetService<IDownloadCommand>(),
                    provider.GetService<IAddCodeCommand>());
            });

            builder.AddTransient<ICommand, DownloadCommand>();
            builder.AddTransient<IDownloadCommand, DownloadCommand>();
            builder.AddTransient<ICommand, AddCodeCommand>();
            builder.AddTransient<IAddCodeCommand, AddCodeCommand>();
            builder.AddTransient<ICommand, ArchiveCommand>();
            builder.AddTransient<ICommand, CreateCommand>();
            builder.AddTransient<ICommand, CombineCommand>();
            builder.AddTransient<ICommand, DownloadAndAddCodeCommand>();

        }

        public static IEnumerable<ICommand> GetCommands()
        {
            return _container.GetServices<ICommand>();
        }

        public static IPtLoggerFactory GetLoggerFactory()
        {
            return _container.GetService<IPtLoggerFactory>();
        }
    }
}