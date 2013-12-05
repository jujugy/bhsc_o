using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Net;

namespace server
{
    static class db
    {
        #region InsertData
        public static bool Data_Save(string _tid)
        {
             string dbCon = "server=localhost;user=root;database=radiyacbaha;port=3307;password=usbw;";
             bool dataCheck;
             MySqlConnection conn = new MySqlConnection(dbCon);
             try
             {
                 conn.Open();
                 
                 string cmdText = "INSERT INTO realtimereco(1TID)VALUES('"+ _tid +"')";
                 MySqlCommand cmd = new MySqlCommand(cmdText, conn);
                 int result = cmd.ExecuteNonQuery();

                 return dataCheck = true;
             }
             catch(WebException ex)
             {
                 return dataCheck = false;
                 //throw ex;
             }
        }
        #endregion

        #region UpdateData1
        public static void Data_Update1(string tid, string data)
        {
            string dbCon = "server=localhost;user=root;database=radiyacbaha;port=3307;password=usbw;";
            MySqlConnection conn = new MySqlConnection(dbCon);
            try
            {
                conn.Open();
                string[] RstArray = data.Split('.');
                string gid, uid, pid, pnum, pnt;
                gid = RstArray[3];
                uid = RstArray[0];
                pid = RstArray[2];
                pnum = RstArray[1];
                pnt = RstArray[5];
                string cmdText = "UPDATE realtimereco SET GID ='" + gid + "', UID ='" + uid + "' , PID ='" + pid + "' , PNUM ='" + pnum + "' , PNT ='" + pnt + "'  WHERE 1TID ='" + tid + "' ";
                MySqlCommand cmd = new MySqlCommand(cmdText, conn);
                int result = cmd.ExecuteNonQuery();

            }
            catch (WebException ex)
            {
                
                //throw ex;
            }
        }
        #endregion

        #region UpdateData2
        public static void Data_Update2(string tid, string data)
        {
            string dbCon = "server=localhost;user=root;database=radiyacbaha;port=3307;password=usbw;";
            MySqlConnection conn = new MySqlConnection(dbCon);
            try
            {
                conn.Open();
                string[] RstArray = data.Split('.');
                string rcode, time, checkcode;
                rcode = RstArray[0];
                time = RstArray[1];
                checkcode = RstArray[2];

                string cmdText = "UPDATE realtimereco SET RCODE ='" + rcode + "', TIME ='" + time + "' , CHECKCODE ='" + checkcode + "' WHERE 1TID ='" + tid + "' ";
                MySqlCommand cmd = new MySqlCommand(cmdText, conn);
                int result = cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch (WebException ex)
            {

                //throw ex;
            }
        }
        #endregion
        
        #region CheckData
        public static string Data_Load(string _tid)
        {
            return null;
        }
        #endregion

    }
}
