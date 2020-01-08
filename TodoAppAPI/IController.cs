using System;
using System.Threading.Tasks;

namespace ToDoAPI
{
    public interface IController
    { 
        Task<string> Manage(string httpMethod, string httpBody, Uri url);
    }
}