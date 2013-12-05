/*using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace ClientConsole {
	class Client {
		static void Main(string[] args) {
			ConsoleKey key;

			ServerClient client = new ServerClient();
			client.SendMessage();
			
			Console.WriteLine("\n\n输入\"Q\"键退出。");
			do {
				key = Console.ReadKey(true).Key;
			} while (key != ConsoleKey.Q);
		}
	}

	public class ServerClient {
		private const int BufferSize = 8192;
		private byte[] buffer;
		private TcpClient client;
		private NetworkStream streamToServer;
		private string msg = ".....adgsadgaewrhad.............!";

		public ServerClient() {
			try {
				client = new TcpClient();
                client.Connect("127.0.0.1", 9988);		
			} catch (Exception ex) {
				Console.WriteLine(ex.Message);
				return;
			}
			buffer = new byte[BufferSize];

			
			Console.WriteLine("Server Connected！{0} --> {1}",
				client.Client.LocalEndPoint, client.Client.RemoteEndPoint);

			streamToServer = client.GetStream();
		}

		
		public void SendMessage(string msg) {

			msg = String.Format("[length={0}]{1}", msg.Length, msg);

			for (int i = 0; i <= 2; i++) {
				byte[] temp = Encoding.Unicode.GetBytes(msg);	
				try {
					streamToServer.Write(temp, 0, temp.Length);	
					Console.WriteLine("Sent: {0}", msg);
				} catch (Exception ex) {
					Console.WriteLine(ex.Message);
					break;
				}
			}

			lock (streamToServer) {
				AsyncCallback callBack = new AsyncCallback(ReadComplete);
				streamToServer.BeginRead(buffer, 0, BufferSize, callBack, null);
			}
		}

		public void SendMessage() {
			SendMessage(this.msg);
		}

		
		private void ReadComplete(IAsyncResult ar) {
			int bytesRead;

			try {
				lock (streamToServer) {
					bytesRead = streamToServer.EndRead(ar);
				}
				if (bytesRead == 0) throw new Exception("读取到0字节");

				string msg = Encoding.Unicode.GetString(buffer, 0, bytesRead);
				Console.WriteLine("Received: {0}", msg);
				Array.Clear(buffer, 0, buffer.Length);	

				lock (streamToServer) {
					AsyncCallback callBack = new AsyncCallback(ReadComplete);
					streamToServer.BeginRead(buffer, 0, BufferSize, callBack, null);
				}
			} catch (Exception ex) {
				if(streamToServer!=null)
					streamToServer.Dispose();
				client.Close();

				Console.WriteLine(ex.Message);
			}
		}
	}


}
*/