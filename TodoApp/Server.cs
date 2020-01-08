using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ToDoAPI
{
    public class Server
    {
        private HttpListener _listener;
        private readonly ItemsController _itemsController = new ItemsController(new ItemsDynamoRepository());
        private readonly UserController _userController = new UserController();

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

                var output = $"Hello, this is the 'to do' list.";

                Console.WriteLine("I got a request!");

                if (req.Url.AbsolutePath.StartsWith("/item"))
                {
                    output = await _itemsController.ManageItems(req.HttpMethod, postRequestBody, req.Url);
                }
                else if (req.Url.AbsolutePath.StartsWith("/user"))
                {
                    output = await _userController.ManageUsers(req.HttpMethod, postRequestBody, req.Url);
                }

                var buffer = System.Text.Encoding.UTF8.GetBytes(output);
                resp.ContentLength64 = buffer.Length;
                await resp.OutputStream.WriteAsync(buffer, 0, buffer.Length);

                resp.Close();
            }
        }
    }
}