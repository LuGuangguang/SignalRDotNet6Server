using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SignalRDotNet6WPFServer.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public IHubContext<SignalRHubs.ChatHub> _hubContext { get; set; }
        public MainWindowViewModel(IHubContext<SignalRHubs.ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public string Msg { get; set; }
        public ICommand SendAllMessageCmd => new RelayCommand(() =>
        {
            SignalRHubs.ChatHub chatHub = new SignalRHubs.ChatHub();
            Models.Student student = new Models.Student() { Name = "李四", Age = 123 };

            _hubContext.Clients.All.SendAsync("ReceiveFromServerMessage", Msg);
        });
        public string Urls { get; set; } = StaticGlobalClass.URLS;
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
