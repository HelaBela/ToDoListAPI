using System;
using System.Net;
using System.Threading.Tasks;
using TodoAppAPI;

namespace ToDoAPI
{
    public interface IController
    { 
        Task<ResponseModel> HandleIncomingRequest(string httpMethod, string httpBody, Uri url);
    }
}