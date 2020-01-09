using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ToDoAPI
{
    public class UserController : IController
    {
        private readonly IRepository<User> _userRepository;

        public UserController(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<string> Manage(string httpMethod, string httpBody, Uri url, HttpListenerResponse resp)
        {
            if (httpMethod == "PUT")
            {
                var user = GetUserFromRequestBody(httpBody);
                user.Id = url.Segments[2];

                await _userRepository.Update(user);
                resp.StatusCode = (int) HttpStatusCode.Accepted;
                return user.ConvertToJson();
            }

            if (httpMethod == "DELETE")
            {
                var id = url.Segments[2];

                await _userRepository.DeleteById(id);
                resp.StatusCode = (int) HttpStatusCode.Accepted;
                return string.Empty;
            }

            if (httpMethod == "GET")
            {
                var all = await _userRepository.RetrieveAll();

                var usersString = JsonConvert.SerializeObject(all);
                resp.StatusCode = (int) HttpStatusCode.OK;
                return usersString;
            }

            if (httpMethod == "POST")
            {
                var user = GetUserFromRequestBody(httpBody);

                user.Id = Guid.NewGuid().ToString();
                resp.StatusCode = (int) HttpStatusCode.OK;
                await _userRepository.Create(user);

                return user.ConvertToJson();
            }

            return string.Empty;
        }

        private static User GetUserFromRequestBody(string httpBody)
        {
            var user = JsonConvert.DeserializeObject<User>(httpBody);
            return user;
        }
    }
}