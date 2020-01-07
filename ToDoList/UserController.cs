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

        public async Task ManageUsers(HttpListenerRequest req, byte[] buffer, HttpListenerResponse resp)
        {
            if (req.HttpMethod == "PUT")
            {
                var user = GetUserFromRequestBody(req);
                user.Id = req.Url.Segments[2];

                await _userRepository.Update(user);
            }
            else if (req.HttpMethod == "DELETE")
            {
                var id = req.Url.Segments[2];

                await _userRepository.DeleteById(id);
            }
            else if (req.HttpMethod == "GET")
            {
                var all = await _userRepository.RetrieveAll();

                var usersString = JsonConvert.SerializeObject(all);
                buffer = System.Text.Encoding.UTF8.GetBytes(usersString);
            }
            else if (req.HttpMethod == "POST")
            {
                var user = GetUserFromRequestBody(req);

                user.Id = Guid.NewGuid().ToString();
                buffer = System.Text.Encoding.UTF8.GetBytes($"Your user Id is {user.Id}");

                await _userRepository.Create(user);
            }

            resp.ContentLength64 = buffer.Length;
            await resp.OutputStream.WriteAsync(buffer, 0, buffer.Length);
        }

        private static User GetUserFromRequestBody(HttpListenerRequest req)
        {
            var reader = new StreamReader(req.InputStream, req.ContentEncoding);
            var postRequestBody = reader.ReadToEnd();
            var user = JsonConvert.DeserializeObject<User>(postRequestBody);
            return user;
        }
    }
}