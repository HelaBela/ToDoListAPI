using System.Collections.Generic;

namespace ToDoAPI
{
    public class TaskList
    {
        public List<ToDoTask> TaskToDoList { get;}
        public TaskList()
        {
            TaskToDoList = new List<ToDoTask>();
        }


        public void Add(ToDoTask toDoTask)
        {
           TaskToDoList.Add(toDoTask);
        }
    }
}