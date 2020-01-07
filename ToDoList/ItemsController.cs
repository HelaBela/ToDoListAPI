using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ToDoAPI
{
    public class ItemsController
    {
        private readonly ToDoTaskRepository _toDoTaskRepository;

        public ItemsController()
        {
            _toDoTaskRepository = new ToDoTaskRepository();
        }

        public async Task ManageItems(HttpListenerRequest req, byte[] buffer, HttpListenerResponse resp)
        {
            if (req.HttpMethod == "PUT")
            {
                var task = GetTaskFromRequestBody(req);
                task.Id = req.Url.Segments[4];

                await _toDoTaskRepository.Update(task);
            }
            else if (req.HttpMethod == "DELETE")
            {
                var id = req.Url.Segments[2];

                await _toDoTaskRepository.DeleteById(id);
            }
            else if (req.HttpMethod == "GET")
            {
                var all = await _toDoTaskRepository.RetrieveAll();

                var itemsString = JsonConvert.SerializeObject(all);
                buffer = System.Text.Encoding.UTF8.GetBytes(itemsString);
            }
            else if (req.HttpMethod == "POST")
            {
                var task = GetTaskFromRequestBody(req);

                task.Id = Guid.NewGuid().ToString();
                buffer = System.Text.Encoding.UTF8.GetBytes($"Your task Id is {task.Id}");

                await _toDoTaskRepository.Create(task);
            }

            resp.ContentLength64 = buffer.Length;
            await resp.OutputStream.WriteAsync(buffer, 0, buffer.Length);
        }

        private static ToDoTask GetTaskFromRequestBody(HttpListenerRequest req)
        {
            var reader = new StreamReader(req.InputStream, req.ContentEncoding);
            var postRequestBody = reader.ReadToEnd();
            var task = JsonConvert.DeserializeObject<ToDoTask>(postRequestBody);
            return task;
        }
    }
}