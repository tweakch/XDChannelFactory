using Configuration.Properties;
using TheCodeKing.Net.Messaging;

namespace Configuration
{
    public interface IXDChannel
    {
        IXDBroadcast CreateBroadcast();

        IXDListener CreateListener(XDListener.XDMessageHandler handler);

        XDTransportMode Mode { get; }

        string Name { get; }

        bool Propagate { get; }
    }

    public class XDChannelFactory
    {
        public static IXDChannel GetLocalChannel(string name)
        {
            return new XDChannelImpl(name, false);
        }

        public static IXDChannel GetNetworkChannel(string name)
        {
            return new XDChannelImpl(name, true);
        }
    }

    public class XDChannelImpl : IXDChannel
    {
        public XDChannelImpl(string name, bool propagatesNetwork) :
            this(name, Settings.Default.TransportMode, propagatesNetwork)
        {
        }

        public XDChannelImpl(string name, XDTransportMode mode, bool propagatesNetwork)
        {
            Name = name;
            Mode = mode;
            Propagate = propagatesNetwork;
        }

        public IXDBroadcast CreateBroadcast()
        {
            return XDBroadcast.CreateBroadcast(Mode, Propagate);
        }

        public IXDListener CreateListener(XDListener.XDMessageHandler handler)
        {
            var listener = XDListener.CreateListener(Mode, !Propagate);
            listener.RegisterChannel(Name);
            listener.MessageReceived += handler;
            return listener;   
        }

        public XDTransportMode Mode
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }
        public bool Propagate
        {
            get;
            private set;
        }
    }
}