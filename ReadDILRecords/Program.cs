using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Collections.Generic;

namespace ReadDILRecords
{
    class Program
    {
        static string conString = @"" + Properties.Settings.Default.ConString;
        static void Main(string[] args)
        {
            
            SalesDataService.SalesDetails sd = new SalesDataService.SalesDetails();

            sd.Credentials = new System.Net.NetworkCredential("yumdata", "Yum@1234", "dil-rjcorp");

            string salesDetails = "";
            int lastcounter = getLastCounter();
            sd.SalesData((lastcounter+1), ref salesDetails);
            salesDetails = salesDetails.Replace("<? xml version = \"1.0\" encoding = \"UTF-8\" ?>", "");
            storeXMLData(salesDetails);
        }
        public static void storeXMLData(string xml)
        {
            SqlConnection dbCon = new SqlConnection(conString);
            if (dbCon.State == ConnectionState.Closed)
                dbCon.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand("storeXML", dbCon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@XML", SqlDbType.VarChar).Value = xml;
                    cmd.ExecuteNonQuery();
                }
            }
            catch(Exception ex)
            {

            }
        }
        public static int getLastCounter()
        {
            int count = 0;
            
            SqlConnection dbCon = new SqlConnection(conString);
            if (dbCon.State == ConnectionState.Closed)
                dbCon.Open();
            try
            {
                string strQuery = "SELECT max(Counter_ID) as Total from DIL";
                SqlCommand dbCmd = new SqlCommand(strQuery, dbCon);
                SqlDataReader reader = dbCmd.ExecuteReader();

                while (reader.Read())
                {
                    
                    count = Convert.ToInt32(reader["Total"]);
                }
            }
            catch (Exception ex)
            {

            }
            return count;
        }
    }
}
