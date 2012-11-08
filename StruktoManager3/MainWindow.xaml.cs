using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Windows.Controls.Ribbon;
using System.Diagnostics;
using TheCodeKing.Net.Messaging;
using Configuration.Properties;
using Configuration;

namespace StruktoManager3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        IXDBroadcast broadcast;
        IXDListener listener;

        public MainWindow()
        {
            InitializeComponent();

            var channel = XDChannelFactory.GetLocalChannel("status");
            broadcast = channel.CreateBroadcast();
            listener = channel.CreateListener(listener_MessageReceived);
        }

        private void listener_MessageReceived(object sender, XDMessageEventArgs e)
        {
            MessageBox.Show("Listener closed");
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            broadcast.SendToChannel("commands", "shutdown");
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            var filename = @"C:\Users\aklee\Documents\Visual Studio 2010\Projects\CAT\code\StruktoManager\StruktoManager3\Prototype\StruktoManager3Solution\Listener\bin\Debug\Listener.exe";
            Process.Start(filename);
        }
    }
}
