using System.Diagnostics;
using System.Windows;
using Configuration;
using Microsoft.Windows.Controls.Ribbon;
using TheCodeKing.Net.Messaging;
using Configuration.Properties;

namespace StruktoManager3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        private IXDBroadcast broadcast;
        private IXDListener listener;

        public MainWindow()
        {
            InitializeComponent();

            var channel = XDChannelFactory.GetLocalChannel(Settings.Default.ChannelName_Status);
            broadcast = channel.CreateBroadcast();
            listener = channel.CreateListener(listener_MessageReceived);
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            var filename = @"C:\Users\aklee\Documents\Visual Studio 2010\Projects\CAT\code\StruktoManager\StruktoManager3\Prototype\StruktoManager3Solution\Listener\bin\Debug\Listener.exe";
            Process.Start(filename);
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            broadcast.SendToChannel(Settings.Default.ChannelName_Commands, "shutdown");
        }

        private void listener_MessageReceived(object sender, XDMessageEventArgs e)
        {
            MessageBox.Show("Listener closed");
        }
    }
}