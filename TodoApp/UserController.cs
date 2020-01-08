using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ToDoAPI
{
    public class UserController
    {
        private readonly UserRepository _userRepository;

        public UserController()
        {
            _userRepository = new UserRepository();
        }

        public async Task<string> ManageUsers(string httpMethod, string httpBody, Uri url)
        {
            if (httpMethod == "PUT")
            {
                var user = GetUserFromRequestBody(httpBody);
                user.Id = url.Segments[2];

                await _userRepository.Update(user);
                return string.Empty;
            }
            else if (httpMethod == "DELETE")
            {
                var id = url.Segments[2];

                await _userRepository.DeleteById(id);
                return string.Empty;
            }
            else if (httpMethod == "GET")
            {
                var all = await _userRepository.RetrieveAll();

                var usersString = JsonConvert.SerializeObject(all);
                return usersString;
            }
            else if (httpMethod == "POST")
            {
                var user = GetUserFromRequestBody(httpBody);

                user.Id = Guid.NewGuid().ToString();
                return $"Your user Id is {user.Id}";

                await _userRepository.Create(user);
            }

            return string.Empty;
        }

        private static User GetUserFromRequestBody(string httpBody)
        {
//            var reader = new StreamReader(req.InputStream, req.ContentEncoding);
//            var postRequestBody = reader.ReadToEnd(); -> i dont understand how come we could move it to server class without issues.
            var user = JsonConvert.DeserializeObject<User>(httpBody);
            return user;
        }
    }
}