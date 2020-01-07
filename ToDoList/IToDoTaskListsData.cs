using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDoAPI
{
    public interface IToDoTaskListsData
    {
        ToDoTask Tasks { get; }

        Task<ToDoTask> Create(ToDoTask toDoTask);
        Task<ToDoTask> Update(ToDoTask toDoTask);
        void DeleteById(string taskId);
        Task<List<ToDoTask>> RetrieveAll();
        ToDoTask RetrieveById(int taskId);
    }
}