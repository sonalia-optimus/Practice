using System;
using System.Net.Sockets;

namespace Sample_Socket
{

    public class Client
    {
        //static public void Main(string[] Args)
        //{
        //    TCPClient socketForServer;
        //    try
        //    {
        //        socketForServer = new TCPClient("localHost", 10);
        //    }
        //    catch
        //    {
        //        Console.WriteLine(
        //        "Failed to connect to server at {0}:999", "localhost");
        //        return;
        //    }
        //    NetworkStream networkStream = socketForServer.GetStream();
        //    System.IO.StreamReader streamReader =
        //    new System.IO.StreamReader(networkStream);
        //    System.IO.StreamWriter streamWriter =
        //    new System.IO.StreamWriter(networkStream);
        //    try
        //    {
        //        string outputString;
        //        // read the data from the host and display it
        //        {
        //            outputString = streamReader.ReadLine();
        //            Console.WriteLine(outputString);
        //            streamWriter.WriteLine("Client Message");
        //            Console.WriteLine("Client Message");
        //            streamWriter.Flush();
        //        }
        //    }
        //    catch
        //    {
        //        Console.WriteLine("Exception reading from Server");
        //    }
        //    // tidy up
        //    networkStream.Close();
        //}
    }
}
