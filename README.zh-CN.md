# SignalRDotNet6Server

---

<div align="center">
<strong><a href="README.md">English</a> | <a href="README.zh-CN.md">简体中文</a></strong>
</div>
<div align="center">
<p> 您的 <a href="https://github.com/LuGuangguang/SignalRDotNet6Server">Star</a> 能帮助 SignaRDotNet6Server 让更多人看到 </p>
</div>

---

### 简介
基于.Net 6.0的SignalR 客户端-服务端 通讯Demo，包含WPF服务端，Web服务端，WPF客户端项目代码

注意：

1.WPF服务端不支持Win7及以下系统，会报Dll错误

### 快速实践

你可以运行  SignalRDotNet31WPFServer 或 SignalRDotNet6WebServer 或 SignalRDotNet6WPFServer 其中一个作为SingalR服务器。

你需要修改 appsettings.json 中 "Urls" 字段为你要配置的服务器地址。

最后你需要启动 WPFCoreSignalRClient 作为客户端来连接SingalR服务端，当然你也需要修改 WPFCoreSignalRClient 下的 appsettings.json 中 "Urls" 字段，保持与服务端的一致。

以上步骤如果你都完成，那么就会在软件UI显示如下图所示：

![signalR](https://user-images.githubusercontent.com/34500722/142980678-be5b3b40-35d8-4efc-95a8-f7c21a173778.png)

