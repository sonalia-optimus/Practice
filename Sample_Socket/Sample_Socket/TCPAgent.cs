using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Sample_Socket
{

    public class TCPAgent
    {
        private string _mPortReading;
        private int _mServConRequest;
        private bool _mPortOpened;
        private string _myRemoteHost;
        private short _myRemotePort;
        private Socket _client;

        private static TCPAgent _instance;

        private TCPAgent() { }

        public static TCPAgent Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TCPAgent();
                }
                return _instance;
            }
        }

        public string PortReading
        {
            get
            {
                return _mPortReading;
            }
            set
            {
                _mPortReading = System.Convert.ToString(value);
            }
        }

        public string NewPortReading
        {
            get
            {
                Read_Port(true);
                return _mPortReading;
            }
            set
            {
                _mPortReading = System.Convert.ToString(value);
            }
        }

        public int Serv_ConRequest
        {
            get
            {
                return _mServConRequest;
            }
            set
            {
                _mServConRequest = System.Convert.ToInt32(value);
            }
        }

        public bool PortOpened
        {
            get
            {
                return _mPortOpened;
            }
            set
            {
                _mPortOpened = System.Convert.ToBoolean(value);
            }
        }

        private Socket Socket
        {
            get
            {
                if (_client == null)
                {
                    _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


                    var ipAddress = IPAddress.Parse("173.195.232.2");
                    var remoteEndPoint = new IPEndPoint(ipAddress, 2101);
                    try
                    {
                        _client.Connect(remoteEndPoint);
                        this.PortOpened = true;
                    }
                    catch (Exception exception)
                    {

                    }
                }
                return _client;
            }
        }

        public string RecievePacket()
        {
            byte[] data = new byte[1024];
            var a = Socket;
            int size = Socket.Receive(data);
            string strPacket = Encoding.UTF8.GetString(data);
            return strPacket;
        }

        public bool IsConnected
        {
            get
            {
                if (Socket == null)
                {
                    return false;
                }
                var retries = 2;
                try
                {
                    while (retries > 0)
                    {
                        if (Socket.Connected) { return true; }
                        retries--;
                        System.Threading.Thread.Sleep(100);
                        _client = null;
                    }
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }


        public bool SocketConnected
        {
            get
            {
                if (Socket == null)
                {
                    return false;
                }
                bool part1 = Socket.Poll(1000, SelectMode.SelectRead);
                bool part2 = (Socket.Available == 0);
                if (part1 && part2)
                    return false;
                else
                    return true;
            }

        }


        public bool Send_TCP(ref string Command_Renamed)
        {
            bool returnValue = false;
            short i;
            try
            {
                if (this.PortOpened)
                {
                    var a = Socket.Blocking;
                    //  WriteUDPData(DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt") + "Inside ReadPort 11111 start");
                   var a1= Socket.Send(Encoding.ASCII.GetBytes(Command_Renamed));
                
                    System.Threading.Thread.Sleep(100);


                    returnValue = true;
                }
            }
            catch (Exception ex)
            {

                returnValue = false;
            }
            return returnValue;
        }

        public bool OpenPort(string RemIP, short RemPort)
        {
            try
            {
                try
                {
                    if (_client != null)
                    {
                        _client.Shutdown(SocketShutdown.Both);
                        _client.Disconnect(true);
                        _client.Close();
                    }
                }
                catch (Exception ex)
                {
                }
                _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                EndPoint ipAddress = new IPEndPoint(IPAddress.Parse(RemIP), RemPort);
               // _client.EnableBroadcast = false;
                //_client.MulticastLoopback = false;
                //_client.DualMode = false;
                _client.Connect(ipAddress);
                Thread.Sleep(100);
                this.PortOpened = true;
            }
            catch (Exception ex)
            {
                this.PortOpened = false;
            }
            return false;
        }

        public void ClosePort()
        {
            try
            {
                _client.Shutdown(SocketShutdown.Both);
                _client.Disconnect(true);
                _client.Close();
            }
            catch (Exception)
            {

            }

            _client = null;
        }

        public string Read_Port(bool dummyMsg)
        {
            string Response = "";
            short L;
            string strRemain = "";
            string strSplitResponse = "";
            short i = 0;
            short j = 0;


            byte[] data = new byte[2000];

            if (dummyMsg)
            {
                if (Socket.Available > 0)
                {
                    int bytesRec = Socket.Receive(data);
                }
            }
            else
            {
                try
                {
                    //Socket.Blocking = false;
                    var a = Socket.Available;
                    int size = Socket.Receive(data, SocketFlags.None);
                }
                catch (SocketException e)
                {
                    Console.WriteLine("{0} Error code: {1}.", e.Message, e.ErrorCode);
                    throw e;
                }

                catch (Exception e)
                {
                    Console.WriteLine("{0} Error code: {1}.", e.Message);
                    throw e;
                }
            }

            Response = Encoding.UTF8.GetString(data);
            return Response;
        }

        public bool Act_Listener(short locPort)
        {
            bool returnValue = false;
            if ((int)this.Serv_ConRequest == 0)
            {
                Socket.Close();
                EndPoint ipAddress = new IPEndPoint(IPAddress.Any, locPort);
                Socket.Connect(ipAddress);
                returnValue = true;
            }
            return returnValue;
        }

    }
}
