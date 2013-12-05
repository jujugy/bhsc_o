using System;
using System.IO;
using System.Web;
using System.Net;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Baha_Demo
{
    public class Util
    {

        #region Post
        public CookieCollection cookie = new CookieCollection();
        public string Httppost(string url, string postData)
        {
            Encoding encoding = Encoding.GetEncoding("utf-8");
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
            return stringResponse;
        }
        #endregion
    }
}