using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using BingMapsRESTToolkit;
using System.Collections.Specialized;
using System.Windows;

namespace RoutingClientTM
{
   
    public enum httpVerb
    {
        GET,
        POST,
        PUT,
        DELETE
    }
    class RestClient
    {
        public string endPoint { get; set; }
        public httpVerb httpMethod { get; set; }
        public string postJson { get; set; }
        public string postRoute { get; set; }
        byte[] response_data { get; set; }
        string res { get; set; }
        byte[] data { get; set; }

        public RestClient()
        {
            endPoint = "";
            httpMethod = httpVerb.GET;
        }

        public string makeRequest()
        {

            string strResponseValue = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endPoint);

            request.Method = httpMethod.ToString();

            
            if(request.Method == "POST" && postJson != string.Empty)
            {
                strResponseValue = PostRequest();
            }
            else
            {
                HttpWebResponse response = null;

                try
                {
                    response = (HttpWebResponse)request.GetResponse();



                    //Proecess the resppnse stream... (could be JSON, XML or HTML etc...)

                    using (Stream responseStream = response.GetResponseStream())
                    {
                        if (responseStream != null)
                        {
                            using (StreamReader reader = new StreamReader(responseStream))
                            {
                                strResponseValue = reader.ReadToEnd();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
                }
                finally
                {
                    if (response != null)
                    {
                        ((IDisposable)response).Dispose();
                    }
                }                
            }
            return strResponseValue;
        }

        public string PostRequest()
        {
            var url = endPoint;
            var client = new WebClient();
            var method = "POST";
            var parameters = new NameValueCollection();
            parameters.Add("Content-Type", "application/x-www-form-urlencoded");
            parameters.Add("route", postJson);
            parameters.Add("itin", postRoute);
            parameters.Add("userid", postJson);
            parameters.Add("date", postRoute);


            try
            {
                /* Always returns a byte[] array data as a response. */
                response_data = client.UploadValues(url, method, parameters);
                res = Encoding.ASCII.GetString(response_data);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
            return res; 
        }


    }
}
