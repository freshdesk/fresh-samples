using System;
using System.Net;
using System.Text;
using System.IO;

class Program
{
    static void Main()
    {
        string fdDomain = "YOUR_DOMAIN"; // your freshdesk domain
        string apiKey = "YOUR_API_KEY";
        string apiPath = "/api/v2/contacts"; // API path
        string responseBody = String.Empty;
        HttpWebRequest request =(HttpWebRequest)WebRequest.Create("https://" + fdDomain + ".freshdesk.com" + apiPath);  
        request.ContentType = "application/json"; 
        request.Method = "GET"; 
        string authInfo = apiKey + ":X"; // It could be your username:password also.
        authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
        request.Headers["Authorization"] ="Basic "+authInfo;

        Console.WriteLine("Submitting Request");
        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        {
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            responseBody = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            //return status code
            Console.WriteLine("Status Code: {1} {0}", ((HttpWebResponse)response).StatusCode, (int)((HttpWebResponse)response).StatusCode);
        }  
        Console.Out.WriteLine(responseBody);

    }
}