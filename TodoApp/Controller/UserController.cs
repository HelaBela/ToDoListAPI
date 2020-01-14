using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ToDoAPI.Controller;
using TodoAppAPI;

namespace ToDoAPI
{
    public class UserController : IController
    {
        private readonly IRepository<User> _userRepository;
        private readonly Dictionary<string, Func<string, Uri, string, Task<ResponseModel>>> _httpMethodHandlers;

        public UserController(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
            _httpMethodHandlers = new Dictionary<string, Func<string, Uri, string, Task<ResponseModel>>>() {
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
                    Body = "{\"Response\": \"Please Choose a different method\"}"
                };
            }
            catch (Exception)
            {
                return new ResponseModel
                {
                    Code = HttpStatusCode.InternalServerError,
                    Body = "{\"Response\": \"Sorry, this is the internal server error.\"}"
                };
            }
        }

        private async Task<ResponseModel> HandleGet(string httpMethod, Uri url, string httpBody)
        {
            var all = await _userRepository.RetrieveAll();
            var itemsString = JsonConvert.SerializeObject(all);
            return new ResponseModel
            {
                Code = HttpStatusCode.OK,
                Body = itemsString
            };
        }

        private async Task<ResponseModel> HandlePost(string httpMethod, Uri url, string httpBody)
        {
            var user = JsonConvert.DeserializeObject<User>(httpBody);
            user.Id = Guid.NewGuid().ToString();
            await _userRepository.Create(user);
            return new ResponseModel
            {
                Code = HttpStatusCode.Created,
                Body = user.ConvertToJson()
            };
        }

        private async Task<ResponseModel> HandlePut(string httpMethod, Uri url, string httpBody)
        {
            if (url.Segments[2] == null ||
                url.Segments[2] != null && !await _userRepository.IsItemIdInDataBase(url.Segments[2]))
            {
                return new ResponseModel
                {
                    Code = HttpStatusCode.NotFound,
                    Body = "{\"Response\": \"Invalid user id.\"}"
                };
            }

            var user = JsonConvert.DeserializeObject<User>(httpBody);
            user.Id = url.Segments[2];
            await _userRepository.Update(user);
            return new ResponseModel
            {
                Code = HttpStatusCode.OK,
                Body = user.ConvertToJson()
            };
        }

        private async Task<ResponseModel> HandleDelete(string httpMethod, Uri url, string httpBody)
        {
            if (url.Segments[2] == null ||
                url.Segments[2] != null && !await _userRepository.IsItemIdInDataBase(url.Segments[2]))
            {
                return new ResponseModel
                {
                    Code = HttpStatusCode.NotFound,
                    Body = "{\"Response\": \"Invalid user id.\"}"
                };
            }

            var id = url.Segments[2];
            await _userRepository.DeleteById(id);
            return new ResponseModel
            {
                Code = HttpStatusCode.NoContent,
                Body = string.Empty
            };
        }
    }
}