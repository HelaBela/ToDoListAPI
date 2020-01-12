using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using TodoAppAPI;

namespace ToDoAPI.tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Should_Create_Item_When_Create_Method_Is_Called()
        {
            //arrange
            var repository = new ItemsInMemoryRepository();
            var item = new Item("1", "1", "test", false);


            //act
            var createdItem = repository.Create(item);
            var allItems = repository.RetrieveAll();

            var newestItem = allItems.Result.Last();

            //assert

            Assert.AreEqual(newestItem.Id, createdItem.Result.Id);
            Assert.AreEqual(newestItem.Title, createdItem.Result.Title);
            Assert.AreEqual(newestItem.IsCompleted, createdItem.Result.IsCompleted);
            Assert.AreEqual(newestItem.UserId, createdItem.Result.UserId);
            Assert.AreEqual(1, allItems.Result.Count);
        }


        [Test]
        public async Task Should_Create_Item_When_POST_Method_Is_Called()
        {
            //arrange
            var repository = new ItemsInMemoryRepository();
            var controller = new ItemsController(repository);
            var httpMethod = "POST";

            var httpBody =
                "{\"UserId\": \"52652099-1300-4a52-b484-6c99b2eb02d0\",\"Title\" : \"be good\",\"IsCompleted\" : true}";

            //act
            var task = controller.HandleIncomingRequest(httpMethod, httpBody, new Uri("http://localhost:8880/item"));

            //assert
            var items = await repository.RetrieveAll();
            Assert.AreEqual(1, items.Count);
        }

        [Test]
        public async Task Should_Delete_Entry_When_DELETE_Method_Is_Called()
        {
            //arrange
            var repository = new ItemsInMemoryRepository();
            var controller = new ItemsController(repository);
            var httpMethod = "POST";

            var httpBody =
                "{\"UserId\": \"52652099-1300-4a52-b484-6c99b2eb02d0\",\"Title\" : \"be good\",\"IsCompleted\" : true}";


            //act
            var task = controller.HandleIncomingRequest(httpMethod, httpBody, new Uri("http://localhost:8880/item"));

            var tasks = await repository.RetrieveAll();

            var item = tasks.FirstOrDefault();

            Assert.IsNotNull(item);

            var itemId = item.Id;

            var task2 = controller.HandleIncomingRequest("DELETE", httpBody,
                new Uri($"http://localhost:8880/item/{itemId}"));

            //assert
            var items = await repository.RetrieveAll();
            Assert.AreEqual(0, items.Count);
        }

        [Test]
        public async Task Should_Update_Entry_When_PUT_Method_Is_Called()
        {
            //arrange
            var repository = new ItemsInMemoryRepository();
            var controller = new ItemsController(repository);
            var httpMethod = "PUT";

            var itemToAmend = new Item("1", "1", "Test", false);

            var httpBody =
                "{\"UserId\": \"52652099-1300-4a52-b484-6c99b2eb02d0\",\"Title\" : \"test\",\"IsCompleted\" : true}";
            await repository.Create(itemToAmend);

            //act
            var task = controller.HandleIncomingRequest(httpMethod, httpBody, new Uri("http://localhost:8880/item/1"));

            var tasks = repository.RetrieveAll();

            var item = tasks.Result.Last();

            Assert.IsNotNull(item);

            //var itemId = item.Title;

            //assert
            Assert.AreEqual(1,tasks.Result.Count );
            Assert.AreEqual("test", item.Title);
            Assert.AreEqual("52652099-1300-4a52-b484-6c99b2eb02d0", item.UserId);
            Assert.IsTrue(item.IsCompleted);
        }
        
        [Test]
        public async Task Should_Retrieve_All_Items_When_GET_Method_Is_Called()
        {
            //arrange
            var repository = new ItemsInMemoryRepository();
            var controller = new ItemsController(repository);
            var httpMethod = "GET";
            
            var item = new Item("1", "1", "test", false);
            await repository.Create(item);

            var httpBody = string.Empty;

                //act
            var task = controller.HandleIncomingRequest(httpMethod, httpBody, new Uri("http://localhost:8880/item/"));

            var tasks = await repository.RetrieveAll();
            
            Assert.IsNotNull(tasks);
            
            var itemsString =  JsonConvert.SerializeObject(tasks);

            //assert

            Assert.AreEqual( "[{\"Id\":\"1\",\"UserId\":\"1\",\"Title\":\"test\",\"IsCompleted\":false}]", itemsString);
           
        }
    }
}