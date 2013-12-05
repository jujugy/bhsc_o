using System;
using System.IO;
using System.Web;
using System.Net;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Globalization;

namespace server
{
    static class Util
    {
        #region post
        public static string GetMD5toBaha(string value){
            MD5 algorithm = MD5.Create();
            byte[] data = algorithm.ComputeHash(Encoding.ASCII.GetBytes(value));
            string md5 = "";
            for (int i = 0; i < data.Length; i++){
                md5 += data[i].ToString("x2").ToLower();
            }
            return md5;
        }
        public static string OneTimesPwd(string strData)
        {
            string K1 = Hash(DateTime.Now.ToString());
            return K1 + Hash(K1 + strData);
        }
        public static string Hash(string strData)
        {
            System.Security.Cryptography.MD5 md5_2 = System.Security.Cryptography.MD5.Create();
            byte[] bytes2 = md5_2.ComputeHash(System.Text.Encoding.UTF8.GetBytes(strData));
            string result2 = string.Empty;
            for (int i = 0; i < bytes2.Length; i++)
            {
                result2 += string.Format("{0:X2}", bytes2[i]);
            }
            return result2;
        }
        public static CookieCollection cookie = new CookieCollection();
        public static string getTime(){ string TIME = DateTime.Now.ToString("yyyyMMddHHmmss"); return TIME;}

        //to baha request more info
        public static string HttpPost(string url, string _GID, string _TID, string _KEY)
        {
            WebRequest request = WebRequest.Create(url);

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
            return null;
        }
        //to radiya
        public static string HttpGet(string url, string _bahaInfo)
        {
            string[] RstArray = _bahaInfo.Split('.');
            int AddPoint;
            string OpenID;
            AddPoint = Int32.Parse(RstArray[1]);
            OpenID = RstArray[0];
            string RadiyaUrl = url + AddPoint + "&openid=" + OpenID + "&key=" + OneTimesPwd(AddPoint + OpenID);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(RadiyaUrl);
            //byte[] buyerInfo = Encoding.ASCII.GetBytes(RadiyaUrl);
            request.Method = "GET";
            request.Accept = "*/*";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Timeout = 150000;
            //request.ContentLength = buyerInfo.Length;   //Count bytes to send

            request.AllowAutoRedirect = false;
            WebResponse response = null;
            string responseStr = null;
            try{
                response = request.GetResponse();

                if (response != null){
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
            }catch (WebException ex){
                throw ex;
            }finally{
                request = null;
                response = null;
            }
            return responseStr;
        }
        //to baha send result
        public static string HttpPost(int sp, string url, string tobaha, string KEY)
        {
            WebRequest request = WebRequest.Create(url);

            string TIME = getTime();
            string RCODE;
            string RMSG = "";
            string[] RstArray = tobaha.Split('.');
            string GID, TID, UID;
            GID = RstArray[3];
            TID = RstArray[4];
            UID = RstArray[0];
            string postData;
            if (sp == 1){
                RCODE = "E000";
                postData = "GID=" + GID + "&TID=" + TID + "&UID=" + UID + "&RCODE=E000&RMSG=&TIME=" + TIME + "&CHECKCODE=" + GetMD5toBaha(KEY + GID + TID + UID + RCODE + TIME).Substring(7, 16).ToLower();
            }else{
                RCODE = "E005";
                postData = "GID=" + GID + "&TID=" + TID + "&UID=" + UID + "&RCODE=E005&RMSG=&TIME=" + TIME + "&CHECKCODE=" + GetMD5toBaha(KEY + GID + TID + UID + RCODE + TIME).Substring(7, 16).ToLower();
            }


            
               Encoding encoding = Encoding.GetEncoding("utf-8");
               byte[] bytesToPost = encoding.GetBytes(postData);
               //CookieContainer cookieCon = new CookieContainer();
               /*if (cookie.Count > 0)
               {
                   cookieCon.Add(new Uri(url), cookie);
               }*/
               HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
               //httpRequest.CookieContainer = cookieCon;
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
               //cookie = httpRequest.CookieContainer.GetCookies(new Uri(url));

            return stringResponse;
            
            /*
            byte[] post = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = post.Length;   //Count bytes to send

            Stream os = null;
            try{ // send the Post
                os = request.GetRequestStream();
                os.Write(post, 0, post.Length);         //Send it
                Console.WriteLine("sending...");
            }
            catch (WebException ex){
                Console.WriteLine("error...");
                throw ex;
            }
            finally{
                if (os != null){
                    os.Close();
                }
            }

            try{
                Stream response = request.GetResponse().GetResponseStream();
                StreamReader sr = new StreamReader(response, Encoding.UTF8);
                string result = sr.ReadToEnd();
                sr.Close();
                Split4(result);
                return result;

            }catch (WebException ex){
                Console.WriteLine("error...");
                throw ex;
            }
            return null;*/
        }
        #endregion
        
        //split first msg from baha
        #region Split1
        public static string Split1(string _msg)
        {
            string tid = "TID";
            string time = "TIME";
            string checkcode = "CHECKCODE";
            Regex regex = new Regex("=");
            string[] parts = regex.Split(_msg);
            tid = parts[1].Split('&')[0];
            time = parts[2].Split('&')[0];
            checkcode = parts[3];
            string result = tid; //+ "." + time + "." + checkcode + ".";
            System.IO.File.WriteAllText(@"D:\FirstMsg.txt", result); //save file
            return result;
        }
        #endregion

        //split second msg from baha
        #region Split2
        public static string Split2(string _msg)
        {
            string rcode, rmsg, gid, tid, uid, server, pid, pnum, pnt, time, checkcode;
            Regex regex = new Regex("=");
            string[] parts = regex.Split(_msg);
            rcode = parts[1].Split('&')[0];
            rmsg = parts[2].Split('&')[0];
            gid = parts[3].Split('&')[0];
            tid = parts[4].Split('&')[0];
            uid = parts[5].Split('&')[0];
            server = parts[6].Split('&')[0];
            pid = parts[7].Split('&')[0];
            pnum = parts[8].Split('&')[0];
            pnt = parts[9].Split('&')[0];
            time = parts[10].Split('&')[0];
            checkcode = parts[11];
            string resultToRadiya = uid + "." + pnum + "." + pid + "." + gid + "." + tid + "." + pnt;//rcode + "." + rmsg + "." + gid + "." + tid + "." + uid + "." + server + "." + pnum + "." + pnt + "." + time + "." + checkcode;
            string resultTofile = rcode + "." + rmsg + "." + gid + "." + tid + "." + uid + "." + server + "." + pnum + "." + pnt + "." + time + "." + checkcode;
            System.IO.File.WriteAllText(@"D:\SecondMsg.txt", resultTofile); //save file
            return resultToRadiya;
        }
        #endregion

        //split msg from radiya
        #region Split3
        public static string Split3(string _msg)
        {
            string result;
            result = _msg.Split(':')[1].Split('}')[0];
            result = result.Split('"')[1].Split('"')[0];
            //System.IO.File.WriteAllText(@"D:\radiyaMsg.txt", result); //save file
            return result;
        }
        #endregion

        //split finial msg from baha
        #region Split4
        public static string Split4(string _msg)
        {
            string rcode, rmsg, gid, tid, uid, time, checkcode;
            Regex regex = new Regex("=");
            string[] parts = regex.Split(_msg);
            rcode = parts[1].Split('&')[0];
            rmsg = parts[2].Split('&')[0];
            gid = parts[3].Split('&')[0];
            tid = parts[4].Split('&')[0];
            uid = parts[5].Split('&')[0];
            time = parts[6].Split('&')[0];
            checkcode = parts[7];
            string fullfinishTrade = rcode + "." + rmsg + "." + gid + "." + tid + "." + uid + "." + time + "." + checkcode;
            System.IO.File.WriteAllText(@"D:\endMsg.txt", fullfinishTrade); //save file
            string finishTrade = rcode + "." + time + "." + checkcode;
            return finishTrade;
        }
        #endregion

    }
}