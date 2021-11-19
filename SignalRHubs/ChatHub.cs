using Microsoft.AspNetCore.SignalR;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SignalRHubs
{
    /// <summary>
    /// 三种基本的发送报文方式
    /// </summary>
    public interface IChatClient
    { 
        Task SendSelfInfoMessage(string msg);
        Task SendSelfMessage(Student student);
        Task SendAllMessage(Student student);
        Task SendGroupMessage(Student student);
    }
    public class UserHandler : INotifyPropertyChanged
    {
        public UserHandler()
        {
            ConnectedIds.CollectionChanged += (sender, e) =>
            {

            };
        }
        public static ObservableCollection<string> ConnectedIds { get; set; } = new ObservableCollection<string>();
        private static string filterString = "aaa";

        public event PropertyChangedEventHandler? PropertyChanged;

        public static string FilterString
        {
            get { return filterString; }
            set { filterString = value; }
        }
    }
    public class ChatHub : Hub<IChatClient>, IChatClient  //此继承为了强类型方法
    {

        public override async Task OnConnectedAsync()
        {
            UserHandler.ConnectedIds.Add(Context.ConnectionId);
            await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            UserHandler.ConnectedIds.Remove(Context.ConnectionId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnDisconnectedAsync(exception);
        }
        public async Task SendSelfInfoMessage(string msg)
        {
            await Clients.Caller.SendSelfInfoMessage(msg);
        }
        public async Task SendSelfMessage(Student student)
        {
            student.ConnectionId = Context.ConnectionId;
            student.Age++;
            await Clients.Caller.SendSelfMessage(student);
        }

        public async Task SendAllMessage(Student student)
        {

            await Clients.All.SendAllMessage(student);

        }
        public async Task SendGroupMessage(Student student)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, student.Grade.ToString());
            student.ConnectionId = Context.ConnectionId;
            await Clients.Groups(new List<string>() { student.Grade.ToString() }).SendGroupMessage(student);

        }


    }
}