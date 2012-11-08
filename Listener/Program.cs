using System;
using System.Threading;
using TheCodeKing.Net.Messaging;
using Configuration.Properties;
using Configuration;

namespace Listener
{
    public class Listener
    {
        public Listener()
        {
        }
        
        IXDListener listener;
        IXDBroadcast broadcast;

        public bool ShutdownRequested { get; set; }
        public void Start()
        {
            Console.WriteLine("Register Channel 'commands'");
            var channel = XDChannelFactory.GetLocalChannel("commands");
            broadcast = channel.CreateBroadcast(); 
            listener = channel.CreateListener(MessageReceived);
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

        internal void Close()
        {
            broadcast.SendToChannel("status", "closing");
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