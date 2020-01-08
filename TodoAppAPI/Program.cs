using System;
using ToDoAPI;

namespace TodoAppAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running!");
            var itemController = new ItemsController(new ItemsInMemoryRepository());
            var userController = new UserController(new UserDynamoRepository());

            new Server(itemController, userController).Run();
            Console.WriteLine("Not running!");
        }
    }
}