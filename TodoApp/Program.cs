using System;
using ToDoAPI;
using ToDoAPI.Controller;

namespace TodoAppAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running!");
            var itemController = new ItemsController(new ItemsInMemoryRepository());
            var userController = new UserController(new UserInMemoryRepository());

            new Server(itemController, userController).Run();
            Console.WriteLine("Not running!");
        }
    }
}