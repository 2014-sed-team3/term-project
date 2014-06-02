using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;

namespace facebook_pages_crawler
{
    class FBGraph
    {
        public JSONObject FBGraphGet(string url)
        {
            Thread.Sleep(new Random().Next(600, 1000));
            try
            {
                return JSONObject.CreateFromString(Web_data_crawler(url));
            }
            catch
            {
                return null;
            }
        }
        public string result(string url)
        {
            return Web_data_crawler(url);
        }
        private string Web_data_crawler(string url)
        {
            Uri uri = new Uri(url.ToString());
            HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;

            request.Method = "GET";
            try
            {
                using (HttpWebResponse response
                        = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader
                        = new StreamReader(response.GetResponseStream());
                    return reader.ReadToEnd();
                }
            }
            catch (WebException e)
            {
                //為FB修改exception 抓取error的訊息
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    //Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    {
                        string text = new StreamReader(data).ReadToEnd();
                        Console.WriteLine("Error message:" + text);
                        return text;
                        //Console.WriteLine(text);
                    }
                }
                //Console.WriteLine(e.ToString());
                //throw e;
            }
        }
    }
}
