using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SignaRDotNet6WPFServer.AppsettingsModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;

namespace SignaRDotNet6WPFServer
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
        public async void ConfigureServices(IServiceCollection services)
        {
            //注册配置参数
            services.AddSingleton<IConfiguration>(this.configuration);
            services.ConfigureWritable<AppsettingsModels.UserSettings>(configuration.GetSection("UserSettings"));
            services.AddSingleton<Views.MainWindow>();
            services.AddSingleton<ViewModels.MainWindowViewModel>();

            var builder = WebApplication.CreateBuilder();
            string urls = configuration.GetSection("Urls").Value;

            if (string.IsNullOrWhiteSpace(urls))
            {

                urls = $"http://{GetLocalIp()}:5999";
            }
            builder.WebHost.UseUrls(urls);  //This line of code is not necessary,runtime will be added automatically
            StaticGlobalClass.URLS = urls;
            // Add services to the container.
            builder.Services.AddSignalR();
            //添加跨域CORS 服务，默认允许任何域名访问
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("*");
                    });
            });
            var app = builder.Build();

          
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<SignalRHubs.ChatHub>("/ChatHub");
            });
            
            IHubContext<SignalRHubs.ChatHub>? hubContext = app.Services.GetService(typeof(IHubContext<SignalRHubs.ChatHub>)) as IHubContext<SignalRHubs.ChatHub>;
            services.AddSingleton<IHubContext<SignalRHubs.ChatHub>>(hubContext);
      
            await app.RunAsync();
        }

        public string GetLocalIp()
        {
            ///获取本地的IP地址
            string AddressIP = string.Empty;
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    AddressIP = _IPAddress.ToString();
                }
            }
            IPAddress ipAddr = Dns.Resolve(Dns.GetHostName()).AddressList[0];//获得当前IP地址
            string ip = ipAddr.ToString();
            return AddressIP;
        }
    }
}
