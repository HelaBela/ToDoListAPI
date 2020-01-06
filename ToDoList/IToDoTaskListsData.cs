using System.Collections.Generic;
using System.Threading.Tasks;
using ConsoleApp3;

namespace ToDoAPI
{
    public interface IToDoTaskListsData
    {
        ToDoTask Tasks { get; }

        Task<ToDoTask> Create(ToDoTask toDoTask);
        ToDoTask Update(ToDoTask toDoTask);
        ToDoTask DeleteById(int taskId);
        Task<List<ToDoTask>> RetrieveAll();
        ToDoTask RetrieveById(int taskId);
    }
}