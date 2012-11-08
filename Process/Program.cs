using System;
using System.Threading;
using Configuration;
using Configuration.Properties;
using TheCodeKing.Net.Messaging;

namespace Process
{
    public class Program
    {
        private static bool run = true;

        private static void Main(string[] args)
        {
            var someProcess = new SomeClass();
            someProcess.Start();

            Console.WriteLine("Waiting for shutdown signal");

            while (!someProcess.ShutdownRequested)
            {
                Thread.Sleep(1000);
            }

            someProcess.Close();
        }
    }

    public class SomeClass
    {
        private IXDBroadcast broadcast;
        private int instanceNr;
        private IXDListener listener;

        public bool ShutdownRequested { get; set; }

        public void Start()
        {
            Console.WriteLine(string.Format("Register Channel '{0}'", Settings.Default.ChannelName_Commands));
            var channel = XDChannelFactory.GetLocalChannel(Settings.Default.ChannelName_Commands);
            broadcast = channel.CreateBroadcast();
            listener = channel.CreateListener(MessageReceived);
            broadcast.SendToChannel(Settings.Default.ChannelName_Status, "Process started!");
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
                    Console.WriteLine("Shutting down!");
                    break;
            }
        }
    }
}