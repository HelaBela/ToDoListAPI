using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using ToDoAPI;
using ToDoAPI.Controller;
using TodoAppAPI;


namespace ToDoApi.IntegrationTests
{
    public class ComponentTest
    {
        private static void RunServer()
        {
            var itemController = new ItemsController(new ItemsInMemoryRepository());
            var userController = new UserController(new UserDynamoRepository());

            var server = new Server(itemController, userController);

            Thread thread = new Thread(server.Run);
            thread.Start();
        }

        private static async Task StopServer()
        {
            using (var getClient = new HttpClient {BaseAddress = new Uri("http://localhost:8880/")})
            {
                await getClient.GetAsync("done/");
            }
        }

        [Test]
        public async Task Can_Create_Item()
        {
            RunServer();

            //new http request to local host

            using (var client = new HttpClient {BaseAddress = new Uri("http://localhost:8880/")})
            {
                var toDoItem = new Item("1", "2", "test-task", false);
                var requestBody = JsonConvert.SerializeObject(toDoItem);
                var postRequest = await client.PostAsync("item/", new StringContent(requestBody));
                await postRequest.Content.ReadAsStringAsync();
                Assert.AreEqual(HttpStatusCode.Created, postRequest.StatusCode);
                // Act
            }

            using (var getClient = new HttpClient {BaseAddress = new Uri("http://localhost:8880/")})
            {
                var response = await getClient.GetAsync("item/");
                var getResponseData = await response.Content.ReadAsStringAsync();
                var allItems = JsonConvert.DeserializeObject<List<Item>>(getResponseData);
                //Assert
                Assert.AreEqual(1, allItems.Count);
                Assert.AreEqual(200, (int) response.StatusCode);
            }

            await StopServer();
        }

        [Test]
        public async Task Can_Delete_Item()
        {
            RunServer();
            var newItemId = "";
            
            using (var client = new HttpClient {BaseAddress = new Uri("http://localhost:8880/")})
            {
                var toDoItem = new Item("1", "2", "test-task", false);
                var requestBody = JsonConvert.SerializeObject(toDoItem);
                var postRequest = await client.PostAsync("item/", new StringContent(requestBody));
                await postRequest.Content.ReadAsStringAsync();
                // Act
            }

            using (var getClient = new HttpClient {BaseAddress = new Uri("http://localhost:8880/")})
            {
                var response = await getClient.GetAsync("item/");
                var getResponseData = await response.Content.ReadAsStringAsync();
                var allItems = JsonConvert.DeserializeObject<List<Item>>(getResponseData);
                newItemId = allItems.FirstOrDefault()?.Id;
                //Assert
                Assert.AreEqual(1, allItems.Count);
            }

            using (var client = new HttpClient {BaseAddress = new Uri("http://localhost:8880/")})
            {
                var deleteAsync = await client.DeleteAsync($"item/{newItemId}");
                await deleteAsync.Content.ReadAsStringAsync();
                Assert.AreEqual( HttpStatusCode.NoContent, deleteAsync.StatusCode);
                // Act
            }

            using (var getClient = new HttpClient {BaseAddress = new Uri("http://localhost:8880/")})
            {
                var response = await getClient.GetAsync("item/");
                var getResponseData = await response.Content.ReadAsStringAsync();
                var allItems = JsonConvert.DeserializeObject<List<Item>>(getResponseData);
                //Assert
                Assert.AreEqual(0, allItems.Count);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }

            await StopServer();
        }


        [Test]
        public async Task Can_Update_Item()
        {
            RunServer();
            var newItemId = "";

            using (var client = new HttpClient {BaseAddress = new Uri("http://localhost:8880/")})
            {
                var toDoItem = new Item("1", "2", "test-task", false);
                var requestBody = JsonConvert.SerializeObject(toDoItem);
                var postRequest = await client.PostAsync("item/", new StringContent(requestBody));
                await postRequest.Content.ReadAsStringAsync();
                // Act
            }

            using (var getClient = new HttpClient {BaseAddress = new Uri("http://localhost:8880/")})
            {
                var response = await getClient.GetAsync("item/");
                var getResponseData = await response.Content.ReadAsStringAsync();
                var allItems = JsonConvert.DeserializeObject<List<Item>>(getResponseData);
                newItemId = allItems.FirstOrDefault()?.Id;
            }

            using (var putClient = new HttpClient {BaseAddress = new Uri("http://localhost:8880/")})
            {
                var httpBody =
                    "{\"UserId\": \"5\",\"Title\" : \"be good\",\"IsCompleted\" : true}";
               var putRequest =  await putClient.PutAsync($"/item/{newItemId}", new StringContent(httpBody));
               var responseBody = await putRequest.Content.ReadAsStringAsync();
                Assert.AreEqual(HttpStatusCode.OK, putRequest.StatusCode);
                Assert.AreEqual( "{\"Id\":\""+ newItemId +"\",\"UserId\":\"5\",\"Title\":\"be good\",\"IsCompleted\":true}",responseBody);
            }

            using (var getClient = new HttpClient {BaseAddress = new Uri("http://localhost:8880/")})
            {
                var response = await getClient.GetAsync("item/");
                var getResponseData = await response.Content.ReadAsStringAsync();
                var allItems = JsonConvert.DeserializeObject<List<Item>>(getResponseData);
                var newItemTitle = allItems.FirstOrDefault()?.Title;
                var isCompleted = allItems.FirstOrDefault()?.IsCompleted;
                var userId = allItems.FirstOrDefault()?.UserId;
                Assert.AreEqual("be good", newItemTitle);
                Assert.AreEqual(true, isCompleted);
                Assert.AreEqual("5", userId);
            }

            await StopServer();
        }
        
        
        [Test]
        public async Task Delete_With_Wrong_Task_ID_Results_In_NotFound_Response()
        {
            RunServer();

            using (var client = new HttpClient {BaseAddress = new Uri("http://localhost:8880/")})
            {
                var toDoItem = new Item("1", "2", "test-task", false);
                var requestBody = JsonConvert.SerializeObject(toDoItem);
                var postRequest = await client.PostAsync("item/", new StringContent(requestBody));
                await postRequest.Content.ReadAsStringAsync();
            }

            using (var getClient = new HttpClient {BaseAddress = new Uri("http://localhost:8880/")})
            {
                var response = await getClient.GetAsync("item/");
                var getResponseData = await response.Content.ReadAsStringAsync();
                var allItems = JsonConvert.DeserializeObject<List<Item>>(getResponseData);
                //Assert
                Assert.AreEqual(1, allItems.Count);
            }

            using (var client = new HttpClient {BaseAddress = new Uri("http://localhost:8880/")})
            {
                var deleteAsync = await client.DeleteAsync($"item/1232");
                var responseBody = await deleteAsync.Content.ReadAsStringAsync();
                Assert.AreEqual( HttpStatusCode.NotFound, deleteAsync.StatusCode);
                Assert.AreEqual( "{\"Response\": \"Invalid task id.\"}", responseBody);
            }
            
            await StopServer();
        }
        
        [Test]
        public async Task Choosing_Unsupported_Method_Results_In_MethodNotAllowed_StatusCode()
        {
            RunServer();

            using (var client = new HttpClient {BaseAddress = new Uri("http://localhost:8880/")})
            {
                var toDoItem = new Item("1", "2", "test-task", false);
                var requestBody = JsonConvert.SerializeObject(toDoItem);
                var putRequest = await client.PatchAsync("item/", new StringContent(requestBody));
                var responseBody = await putRequest.Content.ReadAsStringAsync();
                Assert.AreEqual(HttpStatusCode.MethodNotAllowed, putRequest.StatusCode);
                Assert.AreEqual("{\"Response\": \"Please Choose a different method\"}", responseBody);
            }

            await StopServer();
        }
        
        [Test]
        public async Task Providing_Invalid_URL_Results_Is_NotFound_StatusCode()
        {
            RunServer();

            using (var client = new HttpClient {BaseAddress = new Uri("http://localhost:8880/")})
            {
                var response = await client.GetAsync("itemsssss/");
                var responseBody = await response.Content.ReadAsStringAsync();
                Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
                Assert.AreEqual("{\"Response\": \"Wrong url.\"}", responseBody);
            }

            await StopServer();
        }



    }
}