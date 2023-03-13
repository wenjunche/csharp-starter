using Openfin.Desktop;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using System.Windows;

namespace OpenFin.WPF.TestHarness
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        WorkspaceManagement workspaceManagement;
        public MainWindow()
        {
            InitializeComponent();
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            string workspaceChannelId = appSettings.Get("workspaceChannelId") ?? Settings.DefaultWorkspaceChannelId;
            // get command line arguments
            string[] args = Environment.GetCommandLineArgs();
            // check if args are empty
            if (args.Length > 1) 
            { 
                workspaceChannelId = args[1];
            }
            else
            {
                PopWindow pw = new();
                // add listener for btnOk click 
                pw.btnOk.Click += (sender, e) =>
                {
                    // get text from textbox
                    workspaceChannelId = pw.PlatformTxt.Text;
                    // close pop window
                    pw.DialogResult = true;
                };
                // show pop window
                pw.ShowDialog();
            }
            workspaceManagement = new WorkspaceManagement(System.Windows.Threading.Dispatcher.CurrentDispatcher, workspaceChannelId);
            var menuDropAlignmentField = typeof(SystemParameters).GetField("_menuDropAlignment", BindingFlags.NonPublic | BindingFlags.Static);
            Action setAlignmentValue = () => {
                if (SystemParameters.MenuDropAlignment && menuDropAlignmentField != null) menuDropAlignmentField.SetValue(null, false);
            };
            setAlignmentValue();
            SystemParameters.StaticPropertyChanged += (sender, e) => { setAlignmentValue(); };
            this.Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            workspaceManagement.Disconnect();
        }

        private void MenuItem_LaunchApp1(object sender, RoutedEventArgs e)
        {
            workspaceManagement.LaunchView("wpfapp1");
        }
        private void MenuItem_LaunchApp2(object sender, RoutedEventArgs e)
        {
            workspaceManagement.LaunchView("wpfapp2");
        }
        private void MenuItem_LaunchApp3(object sender, RoutedEventArgs e)
        {
            workspaceManagement.LaunchView("wpfapp3");
        }

        private void MenuItem_ShowHome(object sender, RoutedEventArgs e)
        {
            workspaceManagement.ShowHome();
        }

        private void MenuItem_HideHome(object sender, RoutedEventArgs e)
        {
            workspaceManagement.HideHome();
        }

        private void MenuItem_ShowStore(object sender, RoutedEventArgs e)
        {
            workspaceManagement.ShowStore();
        }

        private void MenuItem_HideStore(object sender, RoutedEventArgs e)
        {
            workspaceManagement.HideStore();
        }
        private void MenuItem_ShowDock(object sender, RoutedEventArgs e)
        {
            workspaceManagement.ShowDock();
        }
        private void MenuItem_MinimizeDock(object sender, RoutedEventArgs e)
        {
            workspaceManagement.MinimizeDock();
        }
    }
}
