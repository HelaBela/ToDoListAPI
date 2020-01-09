using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using ToDoAPI;

namespace ToDoApi.IntegrationTests
{
    public class ComponentTestExample
    {
        [Test]
        public async Task Should_Add_A_Member_When_A_Post_Request_Is_Received()
        {
            // Arrange
            using (var client = new HttpClient {BaseAddress = new Uri("http://localhost:8880/")})
            {
                var toDoItem = new Item("1", "2", "test-task", false);
                var requestBody = JsonConvert.SerializeObject(toDoItem);
                var postRequest = await client.PostAsync("item", new StringContent(requestBody));
                var newItemId = await postRequest.Content.ReadAsStringAsync();
                
                
                // Act

                using (var getClient = new HttpClient {BaseAddress = new Uri("http://localhost:8880/")})
                {
                    var response = await getClient.GetAsync("item");
                    var getResponseData = await response.Content.ReadAsStringAsync();
                    var allItems = JsonConvert.DeserializeObject<List<Item>>(getResponseData);

                }
            }

           
            
            
        }
        
    }
}