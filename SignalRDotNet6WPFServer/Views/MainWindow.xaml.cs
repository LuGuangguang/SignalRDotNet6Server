using SignalRDotNet6WPFServer.AppsettingsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SignalRDotNet6WPFServer.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public IWritableOptions<AppsettingsModels.UserSettings> _userSettings { get; set; }
        public MainWindow(IWritableOptions<AppsettingsModels.UserSettings> userSettings)
        {
            _userSettings = userSettings;
            InitializeComponent();
            if (_userSettings.Value.IsFirstLaunch == true)
            {
                _userSettings.Value.IsFirstLaunch = false;
                _userSettings.Update(it => it.IsFirstLaunch = _userSettings.Value.IsFirstLaunch);
                //MessageBox.Show("第一次打开本软件");

            }
        }
    }
}
