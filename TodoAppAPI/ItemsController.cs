using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ToDoAPI
{
    public class ItemsController: IController
    {
        private readonly IRepository<Item> _itemsRepository;

        public ItemsController(IRepository<Item> itemsRepository)
        {
            _itemsRepository = itemsRepository;
        }

        public async Task<string> Manage(string httpMethod, string httpBody, Uri url)
        {
            if (httpMethod == "PUT")
            {
                var task = GetTaskFromRequestBody(httpBody);
                task.TaskId = url.Segments[4];

                await _itemsRepository.Update(task);
                return string.Empty;
            }

            if (httpMethod == "DELETE")
            {
                var id = url.Segments[2];

                await _itemsRepository.DeleteById(id);
                return string.Empty;
            }

            if (httpMethod == "GET")
            {
                var all = await _itemsRepository.RetrieveAll();
                var itemsString = JsonConvert.SerializeObject(all);
                return itemsString;
            }

            if (httpMethod == "POST")
            {
                var task = GetTaskFromRequestBody(httpBody);

                task.TaskId = Guid.NewGuid().ToString();

                await _itemsRepository.Create(task);
                return $"Your task Id is {task.TaskId}";
            }

            return string.Empty;
        }

        private static Item GetTaskFromRequestBody(string body)
        {
            var task = JsonConvert.DeserializeObject<Item>(body);
            return task;
        }
    }
}