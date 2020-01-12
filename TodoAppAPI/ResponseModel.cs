using System.Net;

namespace TodoAppAPI
{
    public class ResponseModel
    {
        public HttpStatusCode Code { get; set; }
        public string Body { get; set; }
    }
}