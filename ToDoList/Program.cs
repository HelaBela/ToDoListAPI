using System;

namespace ToDoAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running!");
            Server.Run();
            Console.WriteLine("Not running!");
        }
    }
}