using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Net;
using WPFCoreSignalRClient.Models;
using WPFCoreSignalRClient.AppsettingsModels;

namespace WPFCoreSignalRClient.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly IWritableOptions<AppsettingsModels.UserSettings> _userSettings;
        public MainWindowViewModel(IWritableOptions<AppsettingsModels.UserSettings> userSettings)
        {
            _userSettings = userSettings;
            connection = new HubConnectionBuilder()
                .WithUrl(userSettings.Value.Urls)
                .WithAutomaticReconnect()  //自动重连
                .Build();
            ConnectCmd.Execute(null);
            if (connection.State == HubConnectionState.Disconnected)
            {
                timer.Tick += new EventHandler(async (sender, e) =>
                {
                    if (connection.State == HubConnectionState.Disconnected)
                    {
                        await connection.StartAsync();
                        if (connection.State == HubConnectionState.Connected)
                        {
                            timer.Stop();
                        }
                    }
                });
                timer.Start();
            }

            #region snippet_ClosedRestart
            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };

            connection.On<string>("ReceiveFromServerMessage", (info) =>
            {
                App.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    MessagesList.Add(info);
                }));
            });
            connection.On<string>("SendSelfInfoMessage", (info) =>
            {
                App.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    MessagesList.Add(info);
                }));
            });


            connection.On<Student>("SendSelfMessage", (student) =>
            {
                App.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ConnectionId = student.ConnectionId;
                    var newMessage = $"{student.Name}: {student.Age}";
                    MessagesList.Add(newMessage);
                }));
            });
            // connection.On<Student>("ReceiveSelfMessage", new Action<Student>((student) => new HandleChatHub().HandleReceiveSelfMessage(student)));

            connection.On<Student>("SendAllMessage", (student) =>
            {
                App.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    var newMessage = $"{student.Name}: {student.Age}";
                    MessagesList.Add("ReceiveAllMessage:" + newMessage);
                }));
            });
            connection.On<Student>("SendGroupMessage", (student) =>
            {
                App.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    var newMessage = $"{student.Name}: {student.Age}";
                    MessagesList.Add("ReceiveGroupMessage:" + newMessage);
                }));
            });
            #endregion
        }

        private DispatcherTimer timer = new DispatcherTimer()
        {
            Interval = new TimeSpan(0, 0, 15),

        };
        public string Urls
        {
            get
            {
                return _userSettings.Value.Urls;
            }
        }

        public string ConnectionId { get; set; }
        public string UserName { get; set; }
        public string Msg { get; set; }
        public ObservableCollection<string> MessagesList { get; set; } = new ObservableCollection<string>();
        HubConnection connection;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand ConnectCmd => new RelayCommand(async () =>
          {
              try
              {
                  await connection.StartAsync();
                  MessagesList.Add("Connection started");

              }
              catch (Exception ex)
              {
                  MessagesList.Add(ex.Message);
              }
          });
        public ICommand SendSelfInfoMessageCmd => new RelayCommand(async () =>
        {
            try
            {
                if (connection.State == HubConnectionState.Disconnected)
                {
                    await connection.StartAsync();
                }

                await connection.InvokeAsync("SendSelfInfoMessage",
                  Msg);

            }
            catch (Exception ex)
            {
                MessagesList.Add(ex.Message);
            }
        });
        public ICommand SendSelfMessageCmd => new RelayCommand(async () =>
          {
              try
              {
                  if (connection.State == HubConnectionState.Disconnected)
                  {
                      await connection.StartAsync();
                  }
                  Student student = new Student() { Name = "张三", Age = 11 };

                  await connection.InvokeAsync("SendSelfMessage",
                    student);

              }
              catch (Exception ex)
              {
                  MessagesList.Add(ex.Message);
              }
          });

        public ICommand SendAllMessageCmd => new RelayCommand(async () =>
        {
            try
            {
                if (connection.State == HubConnectionState.Disconnected)
                {
                    await connection.StartAsync();
                }
                Student student = new Student() { Name = "王五", Age = 11 };

                await connection.InvokeAsync("SendAllMessage",
                  student);

            }
            catch (Exception ex)
            {
                MessagesList.Add(ex.Message);
            }
        });

        public Student StudentGroup { get; set; } = new Student() { Name = "刘备", Age = 21, Grade = 1 };
        public ICommand SendGroupMessageCmd => new RelayCommand(async () =>
        {
            try
            {
                if (connection.State == HubConnectionState.Disconnected)
                {
                    await connection.StartAsync();
                }


                await connection.InvokeAsync("SendGroupMessage",
                  StudentGroup);

            }
            catch (Exception ex)
            {
                MessagesList.Add(ex.Message);
            }
        });
    }

}
