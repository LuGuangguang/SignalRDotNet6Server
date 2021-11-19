using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WPFCoreSignalRClient.AppsettingsModels;

namespace WPFCoreSignalRClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public IServiceProvider serviceProvider { get; private set; }
        public IConfiguration configuration { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);  //optional: false   If the file does not exist,will throw ; "ture" not
            this.configuration = builder.Build();
            var serviceCollection = new ServiceCollection();
            //注册并构建服务
            ConfigureServices(serviceCollection);
            this.serviceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = this.serviceProvider.GetRequiredService<Views.MainWindow>();
            mainWindow.DataContext = this.serviceProvider.GetRequiredService<ViewModels.MainWindowViewModel>();
            mainWindow.Show();

        }
        public void ConfigureServices(IServiceCollection services)
        {
            //注册配置参数
            services.AddSingleton<IConfiguration>(this.configuration);
            services.ConfigureWritable<AppsettingsModels.UserSettings>(configuration.GetSection("UserSettings"));
            services.AddSingleton<Views.MainWindow>();
            services.AddSingleton<ViewModels.MainWindowViewModel>();
        }
    }
}
