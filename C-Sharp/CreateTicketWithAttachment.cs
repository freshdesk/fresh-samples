using System;
using System.IO;
using System.Net;
using System.Text;

namespace FreshdeskTest
{
    class CreateTicketWithAttachment
    {

        private const string fdDomain = "YOUR_DOMAIN";
        private const string _APIKey = "YOUR_API_KEY";
        private const string path = "/api/v2/tickets";
        private const string _Url = "https://"+ fdDomain + ".freshdesk.com" + path; 
        
        private static void writeCRLF(Stream o)
        {
            byte[] crLf = Encoding.UTF8.GetBytes("\r\n");
            o.Write(crLf, 0, crLf.Length);
        }
        
        private static void writeBoundaryBytes(Stream o, string b, bool isFinalBoundary)
        {
            string boundary = isFinalBoundary == true ? "--" + b + "--" : "--" + b + "\r\n";
            byte[] d = Encoding.UTF8.GetBytes(boundary);
            o.Write(d, 0, d.Length);
        }
        
        private static void writeContentDispositionFormDataHeader(Stream o, string name)
        {
            string data = "Content-Disposition: form-data; name=\"" + name + "\"\r\n\r\n";
            byte[] b = Encoding.UTF8.GetBytes(data);
            o.Write(b, 0, b.Length);
        }
        
        private static void writeContentDispositionFileHeader(Stream o, string name, string fileName, string contentType)
        {
            string data = "Content-Disposition: form-data; name=\"" + name + "\"; filename=\"" + fileName + "\"\r\n";
            data += "Content-Type: " + contentType + "\r\n\r\n";
            byte[] b = Encoding.UTF8.GetBytes(data);
            o.Write(b, 0, b.Length);
        }
        
        private static void writeString(Stream o, string data)
        {
            byte[] b = Encoding.UTF8.GetBytes(data);
            o.Write(b, 0, b.Length);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Application starting...");

            // Define boundary:
            string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");

            // Web Request:
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(_Url);

            wr.Headers.Clear();
            
            // Method and headers:
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            
            // Basic auth:
            string login = _APIKey + ":X"; // It could be your username:password also.
            string credentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(login));
            wr.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
            
            // Body:
            using (var rs = wr.GetRequestStream())
            {
                // Email:
                writeBoundaryBytes(rs, boundary, false);
                writeContentDispositionFormDataHeader(rs, "email");
                writeString(rs, "example@example.com");
                writeCRLF(rs);
                
                // Subject:
                writeBoundaryBytes(rs, boundary, false);
                writeContentDispositionFormDataHeader(rs, "subject");
                writeString(rs, "Ticket Title");
                writeCRLF(rs);
                
                // Description:
                writeBoundaryBytes(rs, boundary, false);
                writeContentDispositionFormDataHeader(rs, "description");
                writeString(rs, "Ticket description.");
                writeCRLF(rs);

                // Status:
                writeBoundaryBytes(rs, boundary, false);
                writeContentDispositionFormDataHeader(rs, "status");
                writeString(rs, "2");
                writeCRLF(rs);

                // Priority:
                writeBoundaryBytes(rs, boundary, false);
                writeContentDispositionFormDataHeader(rs, "priority");
                writeString(rs, "2");
                writeCRLF(rs);
                
                // Attachment:
                writeBoundaryBytes(rs, boundary, false);
                writeContentDispositionFileHeader(rs, "attachments[]", "x.txt", "text/plain");
                FileStream fs = new FileStream("/path/to/my/file.txt", FileMode.Open, FileAccess.Read);
                byte[] data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
                fs.Close();
                rs.Write(data, 0, data.Length);
                writeCRLF(rs);
                
                // End marker:
                writeBoundaryBytes(rs, boundary, true);
                
                rs.Close();
            
                // Response processing:
                try
                {
                    Console.WriteLine("Submitting Request");
                    var response = (HttpWebResponse)wr.GetResponse();
                    Stream resStream = response.GetResponseStream();
                    string Response = new StreamReader(resStream, Encoding.UTF8).ReadToEnd();
                    //return status code
                    Console.WriteLine("Status Code: {1} {0}", ((HttpWebResponse)response).StatusCode, (int)((HttpWebResponse)response).StatusCode);
                    //return location header
                    Console.WriteLine("Location: {0}", response.Headers["Location"]);
                    //return the response 
                    Console.Out.WriteLine(Response);
                }
                catch (WebException ex)
                {
                    Console.WriteLine("API Error: Your request is not successful. If you are not able to debug this error properly, mail us at support@freshdesk.com with the follwing X-Request-Id");
                    Console.WriteLine("X-Request-Id: {0}", ex.Response.Headers["X-Request-Id"]);
                    Console.WriteLine("Error Status Code : {1} {0}", ((HttpWebResponse)ex.Response).StatusCode, (int)((HttpWebResponse)ex.Response).StatusCode);
                    using (var stream = ex.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {   
                      Console.Write("Error Response: ");
                      Console.WriteLine(reader.ReadToEnd());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR");
                    Console.WriteLine(ex.Message);
                }
            }

        }

    }
}
