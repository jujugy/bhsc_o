using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using MySql.Data;
using MySql.Data.MySqlClient;


namespace ClientConsole {
	class Client {
        
		static void Main(string[] args) {
			string connStr = "server=localhost;user=root;database=test;port=3306;password=usbw;";
        MySqlConnection conn = new MySqlConnection(connStr);
        try
        {
            Console.WriteLine("Connecting to MySQL...");
            conn.Open();
            // Perform database operations

           

            //search
            /*string cmdText = "SELECT COUNT(*) FROM hello WHERE 'id' = 2";
            MySqlCommand cmd = new MySqlCommand(cmdText, conn);

            object result = cmd.ExecuteScalar();
            if (result != null)
            {
                int r = Convert.ToInt32(result);
                Console.WriteLine("Number of mau in the projx_log database is: " + r);
            }*/

            //insert
            /*int _tid = 6;
            string cmdText = "INSERT INTO hello(id)VALUES('" + _tid + "')";
            MySqlCommand cmd = new MySqlCommand(cmdText, conn);
            int result = cmd.ExecuteNonQuery();*/

            //INSERT INTO  `hello` (  `id` ) 
            //VALUES ( 7 )

            //int _tid = 6;
            string cmdText = "UPDATE hello SET num =333, ok = 111 WHERE id = 6";
            MySqlCommand cmd = new MySqlCommand(cmdText, conn);
            int result = cmd.ExecuteNonQuery();


            //UPDATE  `test`.`hello` SET  `num` =  '0123',
            //`ok` =  '055' WHERE  `hello`.`id` =5;

             //Console.WriteLine("Number of mau in the projx_log database is: " + result);
             

            
        }
        catch (Exception ex)
        {
            //Console.WriteLine(ex.ToString());
            Console.WriteLine("testend");
        }
        conn.Close();
        Console.WriteLine("Done.");
        Console.ReadLine();
			
		}
	}
}


/*WebRequest request = WebRequest.Create(url);

            string TIME = getTime();
            string postData = "GID=" + _GID + "&TID=" + _TID + "&TIME=" + TIME + "&CHECKCODE=" + GetMD5toBaha(_KEY + _GID + _TID + TIME).Substring(7, 16);

            byte[] post = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = post.Length;   //Count bytes to send

            Stream os = null;
            try{ // send the Post
                os = request.GetRequestStream();
                os.Write(post, 0, post.Length);         //Send it
                Console.WriteLine("sending...");
            }catch (WebException ex){
                Console.WriteLine("error...");
                throw ex;
            }finally{
                if (os != null){
                    os.Close();
                }
            }
            
            try{ // get the response
                /*Encoding encoding = Encoding.GetEncoding("utf-8");
                byte[] bytesToPost = encoding.GetBytes(postData);
                CookieContainer cookieCon = new CookieContainer();
                if (cookie.Count > 0)
                {
                    cookieCon.Add(new Uri(url), cookie);
                }
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.CookieContainer = cookieCon;
                httpRequest.ContentType = "application/x-www-form-urlencoded";
                httpRequest.Method = "POST";
                httpRequest.ContentLength = bytesToPost.Length;
                Stream requestStream = httpRequest.GetRequestStream();
                requestStream.Write(bytesToPost, 0, bytesToPost.Length);
                requestStream.Close();
                Stream responseStream = httpRequest.GetResponse().GetResponseStream();
                string stringResponse = string.Empty;
                StreamReader responseReader = new StreamReader(responseStream, Encoding.UTF8);
                stringResponse = responseReader.ReadToEnd();
                responseReader.Close();
                cookie = httpRequest.CookieContainer.GetCookies(new Uri(url));
                //Split2(stringResponse);
                //return stringResponse;*/
/*
                Stream response = request.GetResponse().GetResponseStream();
                StreamReader sr = new StreamReader(response, Encoding.UTF8);
                string result = sr.ReadToEnd();
                sr.Close();
                Split2(result);
                return result;
            }
            catch (WebException ex){
                Console.WriteLine("error...");
                throw ex;
                
            }
            return null;*/