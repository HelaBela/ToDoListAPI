using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using ToDoAPI;

namespace ToDoApi.IntegrationTests
{
    public class ComponentTest
    {
        
        [Test]
        public async Task Can_Create_Item()
        {
            var itemController = new ItemsController(new ItemsInMemoryRepository());
            var userController = new UserController(new UserDynamoRepository());

            var server = new Server(itemController, userController);

            Thread thread = new Thread(server.Run);
            thread.Start();

            //new http request to local host
            
            using (var client = new HttpClient {BaseAddress = new Uri("http://localhost:8880/")})
            {
                //helper method ?
                var toDoItem = new Item("1", "2", "test-task", false);
                var requestBody = JsonConvert.SerializeObject(toDoItem);
                var postRequest = await client.PostAsync("item/", new StringContent(requestBody));
                var newItemId = await postRequest.Content.ReadAsStringAsync();
                // Act
            }

            using (var getClient = new HttpClient {BaseAddress = new Uri("http://localhost:8880/")})
            {
                var response = await getClient.GetAsync("item/");
                var getResponseData = await response.Content.ReadAsStringAsync();
                var allItems = JsonConvert.DeserializeObject<List<Item>>(getResponseData);
                //Assert
                Assert.AreEqual(1, allItems.Count);
            }

            using (var getClient = new HttpClient {BaseAddress = new Uri("http://localhost:8880/")})
            {
                await getClient.GetAsync("done/");
            }
        }
    }
}