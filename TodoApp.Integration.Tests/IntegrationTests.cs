using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using ToDoAPI;

namespace ToDoApi.IntegrationTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }
//  [Test]
//        public async Task PostTask()
//        {
//            var thread = new Thread(Server.Run);
//            
//            thread.Start();
//
//
//            using (var toDoListClient = new HttpClient())
//            {
//                toDoListClient.BaseAddress = new Uri("http://localhost:8880/");
//                var task = JsonConvert.SerializeObject(
//                    new ToDoTask() {Id = 1, IsCompleted = true, TaskName = "laundry"});
//
//                var postResponse = await toDoListClient.PostAsync("/items", new StringContent(task));
//
//                var itemResponse =
//                    JsonConvert.DeserializeObject<ToDoTask>(await postResponse.Content.ReadAsStringAsync());
//
//                using (var getClient = new HttpClient {BaseAddress = new Uri("http://localhost:8880/")})
//                {
//                    var getRequest = await getClient.GetAsync("/items");
//
//                    var getResponseData = await getRequest.Content.ReadAsStringAsync();
//
//                    var tasks = JsonConvert.DeserializeObject<ToDoTask>(getResponseData);
//
//                    Assert.AreEqual(201, (int) postResponse.StatusCode);
//                }
//            }
//        }
    }
}