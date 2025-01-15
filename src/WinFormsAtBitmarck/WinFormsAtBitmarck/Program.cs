using Caliburn.Micro;
using Microsoft.Extensions.DependencyInjection;
using WinFormsAtBitmarck.Data;
using WinFormsAtBitmarck.Exceptions;
using WinFormsAtBitmarck.Models;
using WinFormsAtBitmarck.Views;

namespace WinFormsAtBitmarck
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // Initialize Dependency Injection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Initialize Application
            var mainView = serviceProvider.GetService<Form1>() ?? throw new MainViewNotFoundException();
            Application.Run(mainView);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IWindowManager, WindowManager>();

            // Register services and view models
            services.AddTransient<Form1ViewModel>();
            services.AddTransient<ProtocollFormViewModel>();

            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddSingleton<IGreetingRepository, GreetingRepository>();

            services.AddTransient<Form1>();
            services.AddTransient<ProtocollForm>();

            // event aggregator
            services.AddSingleton<IEventAggregator, EventAggregator>();

            // this is the mediatr registration
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
        }
    }
}