using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.IO;

namespace server {        

	class server2 {
		static void Main(string[] args) {
			Console.WriteLine("Server is running ... ");
			IPAddress ip = new IPAddress(new byte[] { xxx, xxx, xxx, xxx});	//ip
			TcpListener listener = new TcpListener(ip, xxx);		//port

			listener.Start();			
			Console.WriteLine("Start Listening ...");

			while (true) {
				TcpClient client = listener.AcceptTcpClient();				
				bahaCash wapper = new bahaCash(client);
			}
		}
	}

	public class bahaCash {
		private TcpClient client;
		private NetworkStream streamToClient;
		private const int BufferSize = 8192;
		public static byte[] buffer;
        public static int bytesRead;
		private RequestHandler handler;

        //check ip address, only accept 60.199.217.*
        private string bahaip = "60199217";
        private string clientip;
        private string[] splitstring;

        //need more info
        private string key = "";  	//key
        private string gid = "";	//gid	
        private string url_requestInfo = "https://user.gamer.com.tw/webcoin/playshop_getdata.php";
        private string url_sendInfo = "http://www.radiya.com.tw/radiya/_call.aspx?addGamerPoint=";
        private string url_rusult = "https://user.gamer.com.tw/webcoin/playshop_result.php";
        public string msg;
        public string firstResult;
        public string secondResult;
        public string finialResult;

       
		public bahaCash(TcpClient client) {  //check ip address
			this.client = client;
            splitstring = client.Client.RemoteEndPoint.ToString().Split('.');
            clientip = splitstring[0] + splitstring[1] + splitstring[2];

            if (clientip == bahaip){
                string time = Util.getTime();
                Console.WriteLine("\nCorrect Connected" + time + "！{0} <-- {1}",
                    client.Client.LocalEndPoint, client.Client.RemoteEndPoint);

                streamToClient = client.GetStream();
                buffer = new byte[BufferSize];

                handler = new RequestHandler();

                AsyncCallback callBack = new AsyncCallback(ReadComplete);
                streamToClient.BeginRead(buffer, 0, BufferSize, callBack, null); //r
            }else{
                string time = Util.getTime();
                Console.WriteLine("\nWrong Connected" + time + "！{0} <-- {1}" + "_" + clientip, 
                    client.Client.LocalEndPoint, client.Client.RemoteEndPoint);
                client.Close();
            }
		} 
              
		private void ReadComplete(IAsyncResult ar) {
			bytesRead = 0;

			try {
				lock(streamToClient) {
					bytesRead = streamToClient.EndRead(ar);
					Console.WriteLine("Reading data, {0} bytes ...", bytesRead);
				}
               
                if (bytesRead > 0 && bytesRead <= 300){
                    msg = Encoding.ASCII.GetString(buffer, 0, bytesRead); //str
                    firstResult = Util.Split1(msg);
                    //if tid is a new one to database**************
                    if(db.Data_Save(firstResult)){                                                      //save tid
                        string fullMsg = Util.HttpPost(url_requestInfo, gid, firstResult, key);         //request full msg
                        secondResult = Util.Split2(fullMsg);
                        db.Data_Update1(firstResult, secondResult);                                     //save data
                        string saveCash = Util.HttpGet(url_sendInfo, secondResult);                     //send info to radiya
                        
                        int savePoint = Convert.ToInt32(Util.Split3(saveCash));

                        string resultMsg = Util.HttpPost(savePoint, url_rusult, secondResult, key);     //send result to baha

                        string splitRM = Util.Split4(resultMsg);
                        if (splitRM.Split('.')[0] == "S000"){
                            db.Data_Update2(firstResult, splitRM);                                      //save data
                            client.Close();
                        }else{
                            throw new Exception("something wrong"); //temp
                        }

                    }else{
                        throw new Exception("same tid in database"); //temp
                    }
                }else{
                        throw new Exception("0"); //temp
                }
                
				Array.Clear(buffer,0,buffer.Length);
			    
				string[] msgArray = handler.GetActualString(msg);	//r

				foreach (string m in msgArray) {
					Console.WriteLine("Received: {0}", m);
					string back = m.ToLower();
				}				
                /*
				lock (streamToClient) {
					AsyncCallback callBack = new AsyncCallback(ReadComplete);
					streamToClient.BeginRead(buffer, 0, BufferSize, callBack, null);
				}*/

			}catch(Exception ex) {
				if(streamToClient!=null)
					streamToClient.Dispose();
				client.Close();
				Console.WriteLine(ex.Message);						
			}
		}
	}
}
