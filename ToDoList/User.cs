using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDoAPI
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; }

        public List<ToDoTask> PersonalTasks;
        // public Task<List<ToDoTask>> ToDoTaskList;

        public User(string id, string name)
        {
            // var db = new ToDoTasksInDynamo();
            //ToDoTaskList = db.RetrieveAll();
            PersonalTasks = new List<ToDoTask>();
            Id = id;
            Name = name;
        }
        
        
    }
}