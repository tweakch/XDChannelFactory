using System.Diagnostics;
using System.Windows;
using Configuration;
using Configuration.Properties;
using Microsoft.Windows.Controls.Ribbon;
using TheCodeKing.Net.Messaging;

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

        private void ButtonSend_Click(object sender, RoutedEventArgs e)
        {
            broadcast.SendToChannel(Settings.Default.ChannelName_Commands, "hello");
        }

        private void ButtonShutdown_Click(object sender, RoutedEventArgs e)
        {
            broadcast.SendToChannel(Settings.Default.ChannelName_Commands, "shutdown");
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(Settings.Default.FilePath_SampleProcess);
        }

        private void listener_MessageReceived(object sender, XDMessageEventArgs e)
        {
            MessageBox.Show(string.Format("Received a message on channel {0}: {1}",e.DataGram.Channel, e.DataGram.Message));
        }
    }
}