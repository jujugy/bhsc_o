/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net; // 引入这两个命名空间，以下同
using System.Net.Sockets;

namespace server
{
    class Servertest
    {
        static void Main(string[] args)
        {
            const int BufferSize = 8192;
            ConsoleKey key;

            Console.WriteLine("Server is running ... ");
            IPAddress ip = new IPAddress(new byte[] { 127,0,0,1});
            TcpListener listener = new TcpListener(ip, 8500);
            listener.Start(); // 开始侦听
            Console.WriteLine("Start Listening ...");
            // 获取一个连接，同步方法，在此处中断
            TcpClient remoteClient = listener.AcceptTcpClient();
            // 打印连接到的客户端信息
            Console.WriteLine("Client Connected！{0} <-- {1}",
            remoteClient.Client.LocalEndPoint, remoteClient.Client.RemoteEndPoint);
            // 获得流
            NetworkStream streamToClient = remoteClient.GetStream();
            do
            {
                // 写入buffer 中
                byte[] buffer = new byte[BufferSize];
                int bytesRead;
                try
                {
                    lock (streamToClient)
                    {
                        bytesRead = streamToClient.Read(buffer, 0, BufferSize);
                    }
                    if (bytesRead == 0) throw new Exception("读取到0 字节");
                    Console.WriteLine("Reading data, {0} bytes ...", bytesRead);
                    // 获得请求的字符串
                    string msg = Encoding.Unicode.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("Received: {0}", msg);
                    // 转换成大写并发送
                    msg = msg.ToLower();
                    buffer = Encoding.Unicode.GetBytes(msg);
                    lock (streamToClient)
                    {
                        streamToClient.Write(buffer, 0, buffer.Length);
                    }
                    Console.WriteLine("Sent: {0}", msg);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    break;
                }
            } while (true);

            

            Console.WriteLine("\n\n enter\"Q\"exit");
            do
            {
                key = Console.ReadKey(true).Key;
            } while (key != ConsoleKey.Q);
            if(key == ConsoleKey.Q){
                streamToClient.Dispose();
                remoteClient.Close();
            }
        }

    }
    // 获得IPAddress 对象的另外几种常用方法：IPAddress ip = IPAddress.Parse("127.0.0.1");IPAddress ip = Dns.GetHostEntry("localhost").AddressList[0];
}*/

/*
using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

public class GetSocket
{
    private static Socket ConnectSocket(string server, int port)
    {
        Socket s = null;
        IPHostEntry hostEntry = null;

        // Get host related information. 
        hostEntry = Dns.GetHostEntry(server);

        // Loop through the AddressList to obtain the supported AddressFamily. This is to avoid 
        // an exception that occurs when the host IP Address is not compatible with the address family 
        // (typical in the IPv6 case). 
        foreach (IPAddress address in hostEntry.AddressList)
        {
            IPEndPoint ipe = new IPEndPoint(address, port);
            Socket tempSocket =
                new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            tempSocket.Connect(ipe);

            if (tempSocket.Connected)
            {
                s = tempSocket;
                break;
            }
            else
            {
                continue;
            }
        }
        return s;
    }

    // This method requests the home page content for the specified server. 
    private static string SocketSendReceive(string server, int port)
    {
        string request = "GET / HTTP/1.1\r\nHost: " + server +
            "\r\nConnection: Close\r\n\r\n";
        Byte[] bytesSent = Encoding.ASCII.GetBytes(request);
        Byte[] bytesReceived = new Byte[256];

        // Create a socket connection with the specified server and port. 
        Socket s = ConnectSocket(server, port);

        if (s == null)
            return ("Connection failed");

        // Send request to the server. 
        s.Send(bytesSent, bytesSent.Length, 0);

        // Receive the server home page content. 
        int bytes = 0;
        string page = "Default HTML page on " + server + ":\r\n";

        // The following will block until te page is transmitted. 
        do
        {
            bytes = s.Receive(bytesReceived, bytesReceived.Length, 0);
            page = page + Encoding.ASCII.GetString(bytesReceived, 0, bytes);
        }
        while (bytes > 0);

        return page;
    }

    public static void Main(string[] args)
    {
        string host;
        int port = 80;

        if (args.Length == 0)
            // If no server name is passed as argument to this program, 
            // use the current host name as the default. 
            host = Dns.GetHostName();
        else
            host = args[0];

        string result = SocketSendReceive(host, port);
        Console.WriteLine(result);

        ConsoleKey key;

        Console.WriteLine("\n\n enter\"Q\"exit");
        do
        {
            key = Console.ReadKey(true).Key;
        } while (key != ConsoleKey.Q);
    }
}*/