using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ToDoAPI.Controller;
using TodoAppAPI;

namespace ToDoAPI
{
    public class Server
    {
        private HttpListener _listener;

        private readonly IController _itemsController;
        private readonly IController _userController;

        public Server(IController itemsController, IController userController)
        {
            _itemsController = itemsController;
            _userController = userController;
        }

        public void Run()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://*:8880/");
            _listener.Start();
            Task listenTask = HandleConnection();
            listenTask.GetAwaiter().GetResult();
            _listener.Close();
        }

        private async Task HandleConnection()
        {
            bool runServer = true;

            while (runServer)
            {
                HttpListenerContext ctx = await _listener.GetContextAsync();
                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse resp = ctx.Response;

                var reader = new StreamReader(req.InputStream, req.ContentEncoding);
                var postRequestBody = reader.ReadToEnd();

                var output = new ResponseModel()
                {
                    Code = HttpStatusCode.OK,
                    Body = string.Empty
                };

                Console.WriteLine("I got a request!");

                if (req.Url.Segments[1] == "item/")
                {
                    output = await _itemsController.HandleIncomingRequest(req.HttpMethod, postRequestBody, req.Url);
                }
                else if (req.Url.Segments[1] == "user/")
                {
                    output = await _userController.HandleIncomingRequest(req.HttpMethod, postRequestBody, req.Url);
                }
                else if (req.Url.Segments[1] == "done/")
                {
                    output.Code = HttpStatusCode.Gone;
                    output.Body = "{\"Response\": \"Bye.\"}";
                    runServer = false;
                }
                else
                {
                    output.Code = HttpStatusCode.NotFound;
                    output.Body = "{\"Response\": \"Wrong url.\"}";
                }

                var buffer = System.Text.Encoding.UTF8.GetBytes(output.Body);
                resp.ContentLength64 = buffer.Length;
                resp.StatusCode = (int) output.Code;
                await resp.OutputStream.WriteAsync(buffer, 0, buffer.Length);

                resp.Close();
            }
        }
    }
}