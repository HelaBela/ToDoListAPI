using System;
using System.Threading.Tasks;
using TodoAppAPI;

namespace ToDoAPI.Controller
{
    public interface IController
    { 
        Task<ResponseModel> HandleIncomingRequest(string httpMethod, string httpBody, Uri url);
    }
}