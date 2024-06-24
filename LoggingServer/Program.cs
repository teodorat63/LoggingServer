using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace LoggingServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:5000/");
            listener.Start();
            Console.WriteLine("Logging server started...");

            while (true)
            {
                HttpListenerContext context = await listener.GetContextAsync();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;

                if (request.HttpMethod == "POST")
                {
                    using (StreamReader reader = new StreamReader(request.InputStream, request.ContentEncoding))
                    {
                        string logMessage = await reader.ReadToEndAsync();
                        Console.WriteLine($"Received log: {DateTime.Now}: {logMessage}");
                    }

                    response.StatusCode = (int)HttpStatusCode.OK;
                }
                else
                {
                    response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                    Console.WriteLine($"Received method that is not allowed {DateTime.Now}");

                }

                response.Close();
            }
        }
    }
}
