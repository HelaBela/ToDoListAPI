using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TodoAppAPI;

namespace ToDoAPI
{
    public class ItemsController : IController
    {
        private readonly IRepository<Item> _itemsRepository;
        private readonly Dictionary<string, Func<string, Uri, string, Task<ResponseModel>>> _httpMethodHandlers;

        public ItemsController(IRepository<Item> itemsRepository)
        {
            _itemsRepository = itemsRepository;
            _httpMethodHandlers = new Dictionary<string, Func<string, Uri, string, Task<ResponseModel>>>()
            {
                {"GET", HandleGet},
                {"POST", HandlePost},
                {"PUT", HandlePut},
                {"DELETE", HandleDelete}
            };
        }

        public async Task<ResponseModel> HandleIncomingRequest(string httpMethod, string httpBody, Uri url)
        {
            try
            {
                if (_httpMethodHandlers.ContainsKey(httpMethod))
                {
                    var handler = _httpMethodHandlers[httpMethod];
                    return await handler(httpMethod, url, httpBody);
                }

                return new ResponseModel
                {
                    Code = HttpStatusCode.MethodNotAllowed,
                    Body = "Please Choose a different method"
                };
            }
            catch (Exception)
            {
                return new ResponseModel
                {
                    Code = HttpStatusCode.InternalServerError,
                    Body = "Sorry, this is the internal server error."
                };
            }
        }

        private async Task<ResponseModel> HandleGet(string httpMethod, Uri url, string httpBody)
        {
            var all = await _itemsRepository.RetrieveAll();
            var itemsString = JsonConvert.SerializeObject(all);
            return new ResponseModel
            {
                Code = HttpStatusCode.OK,
                Body = itemsString
            };
        }

        private async Task<ResponseModel> HandlePost(string httpMethod, Uri url, string httpBody)
        {
            var item = JsonConvert.DeserializeObject<Item>(httpBody);
            item.Id = Guid.NewGuid().ToString();
            await _itemsRepository.Create(item);
            return new ResponseModel
            {
                Code = HttpStatusCode.Created,
                Body = item.ConvertToJson()
            };
        }

        private async Task<ResponseModel> HandlePut(string httpMethod, Uri url, string httpBody)
        {
            if (url.Segments[2] == null ||
                url.Segments[2] != null && !await _itemsRepository.IsItemIdInDataBase(url.Segments[2]))
            {
                return new ResponseModel
                {
                    Code = HttpStatusCode.NotFound,
                    Body = "Invalid task id."
                };
            }

            var item = JsonConvert.DeserializeObject<Item>(httpBody);
            item.Id = url.Segments[2];
            await _itemsRepository.Update(item);
            return new ResponseModel
            {
                Code = HttpStatusCode.OK,
                Body = item.ConvertToJson()
            };
        }

        private async Task<ResponseModel> HandleDelete(string httpMethod, Uri url, string httpBody)
        {
            if (url.Segments[2] == null ||
                url.Segments[2] != null && !await _itemsRepository.IsItemIdInDataBase(url.Segments[2]))
            {
                return new ResponseModel
                {
                    Code = HttpStatusCode.NotFound,
                    Body = "Invalid task id."
                };
            }

            var id = url.Segments[2];
            await _itemsRepository.DeleteById(id);
            return new ResponseModel
            {
                Code = HttpStatusCode.NoContent,
                Body = string.Empty
            };
        }
    }
}