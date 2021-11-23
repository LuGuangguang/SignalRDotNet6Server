using System.Net;
{
    var builder = WebApplication.CreateBuilder(args);
    builder.WebHost.UseUrls($"http://{GetLocalIp()}:5999");
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


    app.Run();
}

static string GetLocalIp()
{
    ///获取本地的IP地址
    string AddressIP = string.Empty;
    foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
    {
        if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
        {
            AddressIP = _IPAddress.ToString();
        }
    }
    return AddressIP;
}