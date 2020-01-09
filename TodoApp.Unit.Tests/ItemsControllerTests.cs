using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ToDoAPI.tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }
        

        [Test]
        public async Task Should_Create_Item_When_POST_Method_Is_Called()
        {
            
            //arrange
            var repository = new ItemsInMemoryRepository();
            var controller = new ItemsController(repository);
            var httpMethod = "POST";

            var httpBody =
                "{\"UserId\": \"52652099-1300-4a52-b484-6c99b2eb02d0\",\"TaskName\" : \"be good\",\"IsCompled\" : true}";

            //act
            var task = controller.Manage(httpMethod, httpBody, new Uri("http://localhost:8880/item"));

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
                "{\"UserId\": \"52652099-1300-4a52-b484-6c99b2eb02d0\",\"TaskName\" : \"be good\",\"IsCompled\" : true}";


            //act
            var task = controller.Manage(httpMethod, httpBody, new Uri("http://localhost:8880/item"));

            var tasks = await repository.RetrieveAll();

            var item = tasks.FirstOrDefault();

            Assert.IsNotNull(item);

            var itemId = item.Id;

            var task2 = controller.Manage("DELETE", httpBody,
                new Uri($"http://localhost:8880/item/{itemId}"));

            //assert
            var items = await repository.RetrieveAll();
            Assert.AreEqual(0, items.Count);
        }
    }
}