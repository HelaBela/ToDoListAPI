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

        public async Task<string> Manage(string httpMethod, string httpBody, Uri url, HttpListenerResponse resp)
        {
            if (httpMethod == "PUT")
            {
                var item = GetTaskFromRequestBody(httpBody);
                item.Id = url.Segments[4];

                await _itemsRepository.Update(item);
                resp.StatusCode = (int) HttpStatusCode.Accepted;
                //200 and same response as post - represent the latest task (updated task) 
                return item.ConvertToJson();
            }

            if (httpMethod == "DELETE")
            {
                var id = url.Segments[2];

                await _itemsRepository.DeleteById(id);
                resp.StatusCode = (int)HttpStatusCode.Accepted;
                return string.Empty;
            }

            if (httpMethod == "GET")
            {
                var all = await _itemsRepository.RetrieveAll();
                var itemsString = JsonConvert.SerializeObject(all);
                resp.StatusCode = (int) HttpStatusCode.OK;
                return itemsString;
            }

            if (httpMethod == "POST")
            {
                var item = GetTaskFromRequestBody(httpBody);

                item.Id = Guid.NewGuid().ToString();

                await _itemsRepository.Create(item);
                resp.StatusCode = (int) HttpStatusCode.OK;
                
                return item.ConvertToJson();
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