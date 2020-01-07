using System;

namespace ToDoAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running!");
            new Server().Run();
            Console.WriteLine("Not running!");
        }
    }
}