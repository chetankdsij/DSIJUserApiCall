using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourCompany.Modules.DSJUserSubscription;
using System.Configuration;
using System.Net;
using System.Web.Script.Serialization;

namespace DSIJUserApiCall
{
    class DSIJUserApiCall
    {
        static void Main(string[] args)
        {
            DSIJUserApiCall objDSIJUserApiCall = new DSIJUserApiCall();
            objDSIJUserApiCall.writeprocessentry("Exe start....................");
            objDSIJUserApiCall.CallNewUserApi();
        }
        void CallNewUserApi()
        {
            try
            {
                DSJUserSubscriptionController objDSJUserSubscriptionController = new DSJUserSubscriptionController();
                DataSet ds = objDSJUserSubscriptionController.GetUserInfoData();
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            writeprocessentry("START New User "+ Convert.ToString(item["Name"]) + " Api Call for USER ID " + Convert.ToString(item["UserId"]) + "");
                            string Response = SendNewUserApi(Convert.ToString(item["Name"]), Convert.ToString(item["Cell"]), Convert.ToString(item["Email"]));
                            writeprocessentry(Response);
                            writeprocessentry("END New User "+ Convert.ToString(item["Name"]) + " Api Call for USER ID " + Convert.ToString(item["UserId"]) + "");
                        }
                    }
                    else
                    {
                        writeprocessentry("No table avaible........");
                    }
                }
                else
                {
                    writeprocessentry("No table avaible........");
                }
            }
            catch (Exception ex)
            {
                writeprocessentry(ex.Message + " " + ex.StackTrace);
            }
        }
        protected string SendNewUserApi(string Name, string MobilePhone, string Email)
        {
            string ResponseString = "";
            HttpWebResponse response = null;
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["Request_Url"].ToString());
                request.Accept = "application/json"; //"application/xml";
                request.Method = "PUT";
                request.Headers.Add("Authorization", ConfigurationManager.AppSettings["Request_AuthKey"].ToString());
                PaymentFailure obj = new PaymentFailure()
                {
                    triggerId = Convert.ToInt64(DateTime.Now.ToString("yyMMddHHmmss")),
                    eventState = ConfigurationManager.AppSettings["EventState"].ToString(),
                    NAME = Name.ToUpper(),
                    MOBILE = MobilePhone.ToUpper(),
                    EMAIL = Email.ToUpper(),
                };
                JavaScriptSerializer jss = new JavaScriptSerializer();
                // serialize into json string
                var myContent = jss.Serialize(obj);

                var data = Encoding.ASCII.GetBytes(myContent);

                request.ContentType = "application/json";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode.ToString() != "OK")
                {
                    writeprocessentry("Failure Request not send to the PayuBiz of transaction id ");
                }
                ResponseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    response = (HttpWebResponse)ex.Response;
                    ResponseString = "Some error occured: " + response.StatusCode.ToString();
                }
                else
                {
                    ResponseString = "Some error occured: " + ex.Status.ToString();
                }
                writeprocessentry("Failure Request not send to the PayuBiz of transaction id" + ex.Message + " ex.StackTrace" + ex.StackTrace);
            }
            return ResponseString;
        }
        public void writeprocessentry(string message)
        {
            try
            {
                string filePath = ConfigurationManager.AppSettings["filePath"];
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                string filename = String.Format("{0:yyyy-MM-dd}.txt", DateTime.Now);
                string path = Path.Combine(filePath, filename);
                if (!File.Exists(path))
                {
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine("");
                    }
                }
                message = message + " " + DateTime.Now.ToLongTimeString();
                using (FileStream fs = new FileStream(filePath + filename, FileMode.Append, FileAccess.Write))
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine("\n");
                    sw.WriteLine(message);
                }
            }
            catch (Exception En)
            {
            }
        }

    }
}
