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
        private readonly ItemsController _itemsController = new ItemsController();
        private readonly UserController _userController = new UserController();

        public  void Run()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://*:8880/");
            _listener.Start();
            Task listenTask = HandleConnection();
            listenTask.GetAwaiter().GetResult();
            _listener.Close();
        }

        private  async Task HandleConnection()
        {
            bool runServer = true;

            while (runServer)
            {
                HttpListenerContext ctx = await _listener.GetContextAsync();
                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse resp = ctx.Response;

                var buffer =
                    System.Text.Encoding.UTF8.GetBytes(
                        $"Hello, this is the 'to do' list.");

                Console.WriteLine("I got a request!");

                if (req.Url.Segments[3].StartsWith("items"))
                {
                    await _itemsController.ManageItems(req, buffer, resp);
                }
                else if (req.Url.Segments[1].StartsWith("user"))
                {
                    await _userController.ManageUsers(req, buffer, resp);
                }
                
                resp.Close();
            }
        }
    }
}