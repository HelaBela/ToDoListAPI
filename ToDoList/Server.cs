using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ConsoleApp3;
using Newtonsoft.Json;

namespace ToDoAPI
{
    public class Server
    {
        private static HttpListener _listener;

        public static void Run()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://*:8880/");
            _listener.Start();
            Task listenTask = HandleConnection();
            listenTask.GetAwaiter().GetResult();
            _listener.Close();
        }

        private static async Task HandleConnection()
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


                await ItemsPath(req, buffer, resp);

                resp.Close();
            }
        }

        private static async Task ItemsPath(HttpListenerRequest req, byte[] buffer, HttpListenerResponse resp)
        {
            if (req.Url.AbsolutePath.StartsWith("/items/"))
            {
            }


            if (req.Url.AbsolutePath == "/items")
            {
                if (req.HttpMethod == "GET")
                {
                    var db = new ToDoTasksInDynamo();

                    var all = await db.RetrieveAll();

                    var itemsString = JsonConvert.SerializeObject(all);
                    buffer = System.Text.Encoding.UTF8.GetBytes(itemsString);
                }
                else if (req.HttpMethod == "POST")
                {
                    //buffer = await Buffer(req);

                    using (var reader = new StreamReader(req.InputStream,
                        req.ContentEncoding))
                    {
                        var postRequestBody = reader.ReadToEnd();
                        var task = JsonConvert.DeserializeObject<ToDoTask>(postRequestBody);
                        task.Id = Guid.NewGuid().ToString();
                        buffer = System.Text.Encoding.UTF8.GetBytes($"Your task Id is {task.Id}");

                        var db = new ToDoTasksInDynamo();
                        await db.Create(task);
                    }
                }

                resp.ContentLength64 = buffer.Length;
                await resp.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            }
        }

//        private static async Task<byte[]> Buffer(HttpListenerRequest req)
//        {
//            byte[] buffer;
//            using (var reader = new StreamReader(req.InputStream,
//                req.ContentEncoding))
//            {
//                var postRequestBody = reader.ReadToEnd();
//                var task = JsonConvert.DeserializeObject<ToDoTask>(postRequestBody);
//                task.Id = Guid.NewGuid().ToString();
//                buffer = System.Text.Encoding.UTF8.GetBytes($"Your task Id is {task.Id}");
//                
//                var db = new ToDoTasksInDynamo();
//                await db.Create(task);
//            }


        // return buffer;
    }
}

