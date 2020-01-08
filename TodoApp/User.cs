using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDoAPI
{
    public class User
    {
        public string Id { get; set; }
        public string UserName { get; }
        
        public User(string id, string userName)
        {
            Id = id;
            UserName = userName;
        }
        
        
    }
}