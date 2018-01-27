using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketTools;

namespace Sample_Socket
{
    public class Comm
    {

        public static Comm KBComm { get; set; }
        // public const string CSTOOLS4_LICENSE_KEY = "FMKLPMFRIKHBMURI";
        public const string CSTOOLS4_LICENSE_KEY = "BMKJQDIQHXKJUJGRIKTLOSHHJQFOYO";
        public const string KB_Signature = "POSLOYALTY";
        public const int KB_Action = 0x1;

        public SocketWrench swTCP { get; set; }

        public string HostIP { get; set; }
        public short HostPort { get; set; }

        public bool boolIsConnected { get; set; }
        public static uint[] crc32Table { get; set; }
       // public static uint[] crc32Table { get; set; }
        public short commSystem { get; set; } //
                                              //It is possible that the site uses all three systems and therefore we need to differentiate
                                              //btw each request
                                              //commSystem=0 -> Kickback
                                              //commSystem=1 -> FuelOnly
                                              //commSystem=2 -> Carwash

        public delegate void CommErrorEventHandler(string ErrorNum, string ErrData);
        public CommErrorEventHandler CommErrorEvent;

        public event CommErrorEventHandler CommError
        {
            add
            {
                CommErrorEvent = (CommErrorEventHandler)Delegate.Combine(CommErrorEvent, value);
            }
            remove
            {
                CommErrorEvent = (CommErrorEventHandler)Delegate.Remove(CommErrorEvent, value);
            }
        }

        public delegate void CommDataEventHandler(string Data);
        public CommDataEventHandler CommDataEvent;

        public event CommDataEventHandler CommData
        {
            add
            {
                CommDataEvent = (CommDataEventHandler)Delegate.Combine(CommDataEvent, value);
            }
            remove
            {
                CommDataEvent = (CommDataEventHandler)Delegate.Remove(CommDataEvent, value);
            }
        }


        public bool IsConnected
        {
            get
            {
                bool returnValue = false;
                returnValue = boolIsConnected;
                return returnValue;
            }
        }


        public bool SendData(string strSend)
        {
            bool returnValue = false;

            swTCP.Blocking = false;
            if (boolIsConnected && swTCP.IsWritable)
            {
                if (commSystem == 1)
                {
                    swTCP.Write(strSend);
                  //  WriteToLog("Send to FuelOnly Server: " + strSend);
                }
                returnValue = true;
            }
            else
            {
               // WriteToLog("Cannot Send to Server: " + strSend);
                returnValue = false;
            }

            return returnValue;
        }

        // Nov 27, 2008: Nicolette modified this function to initialize the connection to
        // KickBack in one function and to check the time out inside this class instead of the POS
        // April 15, 2010: Svetlana added system to make this class completely generic
        //Public Function Initialize(ByVal strHostIP As String, _
        //ByVal intHostPort As Integer) As Boolean
        

        //private uint CRC32(string DataStr)
        //{
        //    uint returnValue = 0;

        //    uint crc32Result = 0;
        //    short i = 0;
        //    short strLen = 0;
        //    short iLookup = 0;

        //    crc32Result = 0xFFFFFFFF;
        //    strLen = (short)DataStr.Length;

        //    for (i = 1; i <= strLen; i++)
        //    {
        //        iLookup = Convert.ToInt16((crc32Result & 0xFF) ^ Strings.Asc(DataStr.Substring(i - 1, 1)));
        //        crc32Result = Convert.ToUInt32(((crc32Result & 0xFFFFFF00) / 0x100) & 16777215); // nasty shr 8 with vb :/
        //        crc32Result = crc32Result ^ Convert.ToUInt32(crc32Table[iLookup]);
        //    }

        //    if (crc32Result < 0)
        //    {
        //        returnValue = ~(crc32Result);
        //    }
        //    else
        //    {
        //        returnValue = crc32Result;
        //    }

        //    return returnValue;
        //}

       
        /* Added by sonali 22-Dec-2017 */
        //private string FormatKickBackRequest(string strRequest)
        //{
        //    string returnValue = "";

        //    string strTemp = "";
        //    uint longTemp = 0;

        //    strTemp = "";
        //    strTemp = strTemp + KB_Signature + "\0" + "\0";
        //    strTemp = strTemp + System.Convert.ToString(Strings.Chr(KB_Action)) + "\0" + "\0" + "\0";

        //    longTemp = Convert.ToUInt32(strRequest.Length);
        //    strTemp = strTemp + System.Convert.ToString(Strings.Chr(Convert.ToInt32(longTemp % 256)));
        //    longTemp = longTemp / 256;
        //    strTemp = strTemp + System.Convert.ToString(Strings.Chr(Convert.ToInt32(longTemp % 256)));
        //    longTemp = longTemp / 256;
        //    strTemp = strTemp + System.Convert.ToString(Strings.Chr(Convert.ToInt32(longTemp % 256)));
        //    longTemp = longTemp / 256;
        //    strTemp = strTemp + System.Convert.ToString(Strings.Chr(Convert.ToInt32(longTemp % 256)));

        //    longTemp = CRC32(strRequest);
        //    strTemp = strTemp + System.Convert.ToString(Strings.Chr(Convert.ToInt32(longTemp % 256)));
        //    longTemp = longTemp / 256;
        //    strTemp = strTemp + System.Convert.ToString(Strings.Chr(Convert.ToInt32(longTemp % 256)));
        //    longTemp = longTemp / 256;
        //    strTemp = strTemp + System.Convert.ToString(Strings.Chr(Convert.ToInt32(longTemp % 256)));
        //    longTemp = longTemp / 256;
        //    strTemp = strTemp + System.Convert.ToString(Strings.Chr(Convert.ToInt32(longTemp % 256)));

        //    longTemp = CRC32(strTemp);
        //    strTemp = strTemp + System.Convert.ToString(Strings.Chr(Convert.ToInt32(longTemp % 256)));
        //    longTemp = longTemp / 256;
        //    strTemp = strTemp + System.Convert.ToString(Strings.Chr(Convert.ToInt32(longTemp % 256)));
        //    longTemp = longTemp / 256;
        //    strTemp = strTemp + System.Convert.ToString(Strings.Chr(Convert.ToInt32(longTemp % 256)));
        //    longTemp = longTemp / 256;
        //    strTemp = strTemp + System.Convert.ToString(Strings.Chr(Convert.ToInt32(longTemp % 256)));

        //    returnValue = strTemp + strRequest;

        //    return returnValue;
        //}

        /*End by sonali 22-Dec-2017 */
    }

 
   
   
    

}


