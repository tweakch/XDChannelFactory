using System;
using System.Threading;
using Configuration;
using TheCodeKing.Net.Messaging;
using Configuration.Properties;

namespace Listener
{
    public class Listener
    {
        private IXDBroadcast broadcast;

        private IXDListener listener;

        public Listener()
        {
        }

        public bool ShutdownRequested { get; set; }

        public void Start()
        {
            Console.WriteLine(string.Format("Register Channel '{0}'",Settings.Default.ChannelName_Commands));
            var channel = XDChannelFactory.GetLocalChannel(Settings.Default.ChannelName_Commands);
            broadcast = channel.CreateBroadcast();
            listener = channel.CreateListener(MessageReceived);
        }

        internal void Close()
        {
            broadcast.SendToChannel(Settings.Default.ChannelName_Status, "closing");
        }

        private void MessageReceived(object sender, XDMessageEventArgs e)
        {
            Console.WriteLine(string.Format("Received message {0}", e.DataGram.Message));
            switch (e.DataGram.Message)
            {
                case "shutdown":
                    ShutdownRequested = true;
                    break;
            }
        }
    }

    public class Program
    {
        private static bool run = true;

        private static void Main(string[] args)
        {
            var listener = new Listener();
            listener.Start();

            Console.WriteLine("Waiting for shutdown signal");

            while (!listener.ShutdownRequested)
            {
                Thread.Sleep(1000);
            }

            listener.Close();
        }
    }
}