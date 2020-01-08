using System;
using System.ComponentModel.DataAnnotations;
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
        public async Task ShouldCreateItem_WhenPostMethodIsCalled()
        {
            //arrange
            var repository = new ItemsInMemoryRepository();
            var controller = new ItemsController(repository);
            var httpMethod = "POST";

            var httpBody =
                "{\"UserId\": \"52652099-1300-4a52-b484-6c99b2eb02d0\",\"TaskName\" : \"be good\",\"IsCompled\" : true}";

            //act
            var task = controller.ManageItems(httpMethod, httpBody, new Uri("http://localhost:8880/item"));

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
            var task = controller.ManageItems(httpMethod, httpBody, new Uri("http://localhost:8880/item"));
            

            var task2 = controller.ManageItems("DELETE", httpBody,
                new Uri("http://localhost:8880/item/52652099-1300-4a52-b484-6c99b2eb02d0"));

            //assert
            var items = await repository.RetrieveAll();
            Assert.AreEqual(0, items.Count);
        }
    }
}