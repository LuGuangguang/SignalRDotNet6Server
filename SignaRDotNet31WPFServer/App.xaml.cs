﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SignaRDotNet31WPFServer.AppsettingsModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SignaRDotNet31WPFServer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public IServiceProvider serviceProvider { get; private set; }
        public IConfiguration configuration { get; private set; }
        private IHost _host;
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
        public async void ConfigureServices(IServiceCollection services)
        {
            //注册配置参数
            services.AddSingleton<IConfiguration>(this.configuration);
            services.ConfigureWritable<AppsettingsModels.UserSettings>(configuration.GetSection("UserSettings"));
            services.AddSingleton<Views.MainWindow>();
            services.AddSingleton<ViewModels.MainWindowViewModel>();

            if (_host != null) _host.Dispose();
            _host = Host.CreateDefaultBuilder().ConfigureWebHostDefaults
               (webBuilder => webBuilder
                    .ConfigureServices(services =>
                    {
                        services.AddSignalR();

                    })
                    .Configure(app =>
                    {
                        app.UseRouting();
                        app.UseEndpoints(endpoints => endpoints.MapHub<SignalRHubs.ChatHub>("/chathub"));
                        // app.UseEndpoints(endpoints => endpoints.MapHub<NebuLogHub>("/NebuLogHub"));
                    }))
               .Build();
            _host.Start();
            #region 从 IHubContext IHost 获取 的实例并依赖，用于从服务端向客户端主动发送请求
            IHubContext<SignalRHubs.ChatHub>? hubContext = _host.Services.GetService(typeof(IHubContext<SignalRHubs.ChatHub>)) as IHubContext<SignalRHubs.ChatHub>;
            services.AddSingleton<IHubContext<SignalRHubs.ChatHub>>(hubContext);
            #endregion
        }
    }
}
