# SignaRDotNet6Server
<div align="center">
<strong><a href="README.md">English</a> | <a href="README.zh-CN.md">简体中文</a> </strong>
</div>

---

### Introduction
SignalR C/S Demo based on .Net 6.0, including WPF server, Web server, WPF client project code;

It can help you build SignalR C/S project fastly and easily;

Especially it is very suitable for the rapid construction and deployment of LAN communication;

### Notice:

1. The WPF server does not support Win7 and below systems and will report a Dll error

2. Currently does not support sending information from the server to the client (had submitted to SignalR Issue)

### Fast Practice

You can run project SignaRDotNet31WPFServer|SignaRDotNet6WebServer|SignaRDotNet6WPFServer one of it to build SignalR Server.

You need to modify appsettings.json-"Urls" Value to change SignalR server Url.

Last you should run WPFCoreSignalRClient Project,and also you should change the project  appsettings.json-"UserSettings"-"Urls",it needs to be consistent the server urls.

If your configuration is correct，the UI will show connect count>0.

![signalR](https://user-images.githubusercontent.com/34500722/142980678-be5b3b40-35d8-4efc-95a8-f7c21a173778.png)





