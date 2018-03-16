using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketTools;
using Microsoft.VisualBasic;
using System.Threading;
using System.Diagnostics;

namespace Sample_Socket
{
    public class Program
    {
        public static int kickBackfalg = 0;
        uint[] table;
        private Object thisLock = new Object();
     
        public static Action OnConnectDelegate;

        public bool Initialize(string strHostIP, short intHostPort, short System_Renamed)
        {

            bool returnValue = false;
           

            int nError = 0;
            float timeIN = 0;
            byte TimeOut = 100;
            var comm = new Comm();
            comm.HostIP = strHostIP;
            comm.HostPort = intHostPort;

            comm.swTCP = new SocketWrench();
            comm.swTCP.OnConnect += new EventHandler(swTCP_OnConnect);
            //Comm.Comm.KBComm.swTCP.OnConnect += (sender2,e2)=> swTCP_OnConnect(sender2,e2,ref Comm Comm.KBComm);
            comm.swTCP.OnDisconnect += new EventHandler(swTCP_OnDisconnect);
            comm.swTCP.OnError += new SocketWrench.OnErrorEventHandler(swTCP_OnError);
            comm.swTCP.OnRead += new EventHandler(swTCP_OnRead);

            OnConnectDelegate = swTCP_OnConnect;
            comm.swTCP.AutoResolve = false;
            comm.swTCP.Blocking = false;
            comm.swTCP.Secure = false;
            // TODO: Updated call of the socket wrench for the new dll. - Ipsit_33
            // Refer: https://sockettools.com/webhelp/dotnet/dotnet/htmlhelp/SocketTools.SocketWrench.Initialize_overload_1.html
            // var result = Comm.KBComm.swTCP.Initialize("FMKLPMFRIKHBMURI");
            var result = comm.swTCP.Initialize("BMKJQDIQHXKJUJGRIKTLOSHHJQFOYO");

            nError = Convert.ToInt32(result);
            if (nError != 1)
            {
                returnValue = false;
                // WriteToLogFile("Failed to initialize SocketWrench control");
                return returnValue;
            }

            // TODO: Updated call of the socket wrench for the new dll. - Ipsit_34
            if (comm.swTCP.Status == SocketWrench.SocketStatus.statusDisconnect ||
                           comm.swTCP.Status == SocketWrench.SocketStatus.statusUnused)
            {
                Comm.KBComm = comm;
                // bool i=Comm.KBComm.swTCP.Connect(Comm.KBComm.HostIP, Comm.KBComm.HostPort, SocketWrench.SocketProtocol.socketStream, TimeOut);

                if (comm.swTCP.Connect(comm.HostIP, comm.HostPort, SocketWrench.SocketProtocol.socketStream, TimeOut))
                {
                    OnConnectDelegate?.Invoke();
                    nError = 1;
                }
                else
                {
                    nError = 0;
                }
                Byte[] b = new Byte[30];
                //string readStream = string.Empty;
                //bool result = Comm.KBComm.swTCP.ReadStream(ref readStream);
                int a1 = comm.swTCP.Write("abcde");
                comm.swTCP.Listen(strHostIP, intHostPort);
                if (comm.swTCP.IsConnected)
                {
                    //lock (p.thisLock)
                    {
                        int s = comm.swTCP.Read(b);
                    }
                    if (comm.swTCP.IsReadable == true)
                    {

                    }
                }
                //nError = Convert.ToInt32();
                returnValue = comm.boolIsConnected;


            }
            return returnValue;
        }
        public static void Main(string[] args)
        {

            Program p = new Program();
            //bool a= p.Initialize("173.195.232.2", (short)2101, (short)0);
            TCPAgent.Instance.OpenPort("173.195.232.2", 2101);
            Byte[] b = new Byte[30];
            //string readStream = string.Empty;
            //bool result = Comm.KBComm.swTCP.ReadStream(ref readStream);
            //int a1= Comm.KBComm.swTCP.Write("abcde");
            string strSend = "<GetRewardsRequest><RequestHeader><PosLoyaltyInterfaceVersion>1.0.0</PosLoyaltyInterfaceVersion><VendorName>InfoNet-Tech</VendorName><VendorModelVersion>Pos 3.00.01</VendorModelVersion><POSSequenceID>40464</POSSequenceID><LoyaltySequenceID/><StoreLocationID>GAS1</StoreLocationID><LoyaltyOfflineFlag value='no'/></RequestHeader><LoyaltyID entryMethod='swipe'>6034311109307726</LoyaltyID></GetRewardsRequest>";

            string s = p.FormatKickBackRequest(strSend);
            string text = "abcde";
            
            if (TCPAgent.Instance.IsConnected)
            {
                TCPAgent.Instance.Send_TCP(ref s);
               var response= TCPAgent.Instance.RecievePacket();
             //   var response = TCPAgent.Instance.Read_Port(false);
            }
            //if (Comm.KBComm.swTCP.IsConnected)
            //{
            //    lock (p.thisLock)
            //    {
            //        int s = Comm.KBComm.swTCP.Read(b);
            //    }
            //    if (Comm.KBComm.swTCP.IsReadable == true)
            //    {

            //    }
            //}
            Console.ReadLine();
        }
        public void swTCP_OnConnect()
        {
            //var comm = new Comm();
            Comm.KBComm.boolIsConnected = true;
            var a = Comm.KBComm.swTCP.LastErrorString;
        }
        public void swTCP_OnConnect(Object eventSender, EventArgs eventArgs)
        {
            //var comm = new Comm();
            Comm.KBComm.boolIsConnected = true;
            var a = Comm.KBComm.swTCP.LastErrorString;
        }

        public void swTCP_OnDisconnect(Object eventSender, EventArgs eventArgs)
        {
            // var comm = new Comm();
            Comm.KBComm.boolIsConnected = false;
            Comm.KBComm.swTCP.Disconnect();
        }

        public void swTCP_OnError(Object eventSender, SocketWrench.ErrorEventArgs eventArgs)
        {
            //var comm = new Comm();
            //  if (!string.IsNullOrEmpty(comm.CommErrorEvent(eventArgs.Error.ToString(), eventArgs.Description)))
            Comm.KBComm.CommErrorEvent?.Invoke(eventArgs.Error.ToString(), eventArgs.Description);

            //riteToLogFile("Error in Comm class. Error is " + eventArgs.Error + " " + eventArgs.Description);

        }

        public void swTCP_OnRead(Object eventSender, EventArgs eventArgs)
        {
            var comm = new Comm();
            string RespStr = "";

            if (Comm.KBComm.swTCP.IsReadable == true)
            {
                Comm.KBComm.swTCP.Read(ref RespStr, 4096);
                //
                if (Comm.KBComm.commSystem == 0 | Comm.KBComm.commSystem == 1)
                {
                    Comm.KBComm.CommDataEvent?.Invoke(RespStr.Substring(28));
                }
                else
                {
                    Comm.KBComm.CommDataEvent?.Invoke(RespStr);
                }
            }

            if (Comm.KBComm.commSystem == 1)
            {
                //WriteToLogFile("Received from FuelOnly Server: " + RespStr);
            }


        }

        public string FormatKickBackRequest(string strRequest)
        {

            var comm = new Comm();

            string returnValue = "";


            string strTemp = "";
            uint longTemp = 0;

            strTemp = "";
            strTemp = strTemp + Comm.KB_Signature + "\0" + "\0";
            strTemp = strTemp + System.Convert.ToString(Convert.ToChar(Comm.KB_Action)) + "\0" + "\0" + "\0";

            longTemp = Convert.ToUInt32(strRequest.Length);
            strTemp = strTemp + System.Convert.ToString(Convert.ToChar(Convert.ToInt32(longTemp % 256)));
            longTemp = longTemp / 256;
            strTemp = strTemp + System.Convert.ToString(Convert.ToChar(Convert.ToInt32(longTemp % 256)));
            longTemp = longTemp / 256;
            strTemp = strTemp + System.Convert.ToString(Convert.ToChar(Convert.ToInt32(longTemp % 256)));
            longTemp = longTemp / 256;
            strTemp = strTemp + System.Convert.ToString(Convert.ToChar(Convert.ToInt32(longTemp % 256)));

            longTemp = CRC32(strRequest);
            strTemp = strTemp + System.Convert.ToString(Convert.ToChar(Convert.ToInt32(longTemp % 256)));
            longTemp = longTemp / 256;
            strTemp = strTemp + System.Convert.ToString(Convert.ToChar(Convert.ToInt32(longTemp % 256)));
            longTemp = longTemp / 256;
            strTemp = strTemp + System.Convert.ToString(Convert.ToChar(Convert.ToInt32(longTemp % 256)));
            longTemp = longTemp / 256;
            strTemp = strTemp + System.Convert.ToString(Convert.ToChar(Convert.ToInt32(longTemp % 256)));

            longTemp = CRC32(strTemp);
            strTemp = strTemp + System.Convert.ToString(Convert.ToChar(Convert.ToInt32(longTemp % 256)));
            longTemp = longTemp / 256;
            strTemp = strTemp + System.Convert.ToString(Convert.ToChar(Convert.ToInt32(longTemp % 256)));
            longTemp = longTemp / 256;
            strTemp = strTemp + System.Convert.ToString(Convert.ToChar(Convert.ToInt32(longTemp % 256)));
            longTemp = longTemp / 256;
            strTemp = strTemp + System.Convert.ToString(Convert.ToChar(Convert.ToInt32(longTemp % 256)));

            returnValue = strTemp + "="+ strRequest;
            // var a7 = Comm.KBComm.swTCP.IsConnected;
            return returnValue;
        }



      


        public void Class_Initialize_Renamed()
        {
            //'  This is the official polynomial used by CRC32 in PKZip.
            //'  Often the polynomial is shown reversed (04C11DB7).
            // var comm = new Comm();
            // const int dwPolynomial = unchecked((int)0xEDB88320);
            const int dwPolynomial = unchecked((int)0xEDB88320);
            short i = 0;
            short j = 0;
            int dwCrc = 0;
            if (kickBackfalg == 1)
            {


                Comm.crc32Table = new uint[257];

                for (i = 0; i <= 255; i++)
                {
                    dwCrc = i;
                    for (j = 8; j >= 1; j--)
                    {
                        if ((dwCrc & 1) == 1)
                        {
                            // dwCrc = System.Convert.ToInt32(((dwCrc & 0xFFFFFFFE) / 2) & 0x7FFFFFFF);
                            dwCrc = (dwCrc >> 1) ^ dwPolynomial;
                            // dwCrc = dwCrc ^ dwPolynomial;
                        }
                        else
                        {
                            dwCrc >>= 1;
                            // dwCrc = System.Convert.ToInt32(((dwCrc & 0xFFFFFFFE) / 2) & 0x7FFFFFFF);
                        }
                    }
                    Comm.crc32Table[i] = (uint)dwCrc;
                }

                //  Comm.KBComm.HostIP = "216.83.78.100";
                //Comm.KBComm.HostPort = (short)701;




            }

        }

        public uint CRC32(string DataStr)
        {
            var a = Comm.KBComm;
            kickBackfalg = 1;
            Class_Initialize_Renamed();
            var comm = new Comm();
            uint returnValue = 0;
          

            uint crc32Result = 0;
            // ulong crc32Result = 0;
            short i = 0;
            short strLen = 0;
            // short iLookup = 0;
            ulong iLookup = 0;
            byte[] array = Encoding.ASCII.GetBytes(DataStr);
            crc32Result = 0xFFFFFFFF;
            strLen = (short)DataStr.Length;

            for (i = 0; i < strLen; i++)
            {
                //var a = DataStr.Substring(i - 1, 1).ToCharArray();
                //char c = a[0];
                //int f = Convert.ToInt32(c); 

                //iLookup = Convert.ToInt16((crc32Result & 0xFF) ^ Convert.ToInt32(DataStr.Substring(i - 1, 1)));
                //crc32Result = Convert.ToUInt32(((crc32Result & 0xFFFFFF00) / 0x100) & 16777215); // nasty shr 8 with vb :/
                // crc32Result = crc32Result ^ Convert.ToUInt32(crc32Table[iLookup]);
                iLookup = (crc32Result & 0xFF) ^ array[i];
                crc32Result >>= 8;
                // var a = Comm.KBComm.crc32Table[iLookup];
                crc32Result ^= Comm.crc32Table[iLookup];

                //crc32Result = crc32Result ^ a2;
            }

            if (crc32Result < 0)
            {
                returnValue = ~(crc32Result);
            }
            else
            {
                returnValue = crc32Result;
            }

            kickBackfalg = 0;
            return returnValue;
        }


        /*------------------------------------------------------*/





        //public uint ComputeChecksumBytes(string s)
        //{
        //    byte[] bytes = Encoding.ASCII.GetBytes(s);
        //    CRC32();
        //    uint crc = 0xffffffff;
        //    for (int i = 0; i < bytes.Length; ++i)
        //    {
        //        byte index = (byte)(((crc) & 0xff) ^ bytes[i]);
        //        crc = (uint)((crc >> 8) ^ table[index]);
        //    }


        //    return ~crc;
        //}

        //public void CRC32()
        //{
        //    uint poly = 0xedb88320;
        //    table = new uint[256];
        //    uint temp = 0;
        //    for (uint i = 0; i < table.Length; ++i)
        //    {
        //        temp = i;
        //        for (int j = 8; j > 0; --j)
        //        {
        //            if ((temp & 1) == 1)
        //            {
        //                temp = (uint)((temp >> 1) ^ poly);
        //            }
        //            else
        //            {
        //                temp >>= 1;
        //            }
        //        }
        //        table[i] = temp;
        //    }

        //}

    }
}
