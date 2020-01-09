using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ToDoAPI
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; }
        
        public User(string id, string name)
        {
            Id = id;
            Name = name;
        }
        
        public string ConvertToJson()
        {
            return  JsonConvert.SerializeObject(this);
        }
        
        
    }
}