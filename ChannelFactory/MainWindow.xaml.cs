using System.Diagnostics;
using System.Windows;
using Configuration;
using Microsoft.Windows.Controls.Ribbon;
using TheCodeKing.Net.Messaging;
using Configuration.Properties;

namespace ChannelFactory
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
            Process.Start(Settings.Default.FilePath_SampleProcess);
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