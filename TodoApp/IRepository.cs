using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDoAPI
{
    public interface IRepository<T>
    {
        Task<T> Create(T entity);
        Task<T> Update(T entity);
        Task DeleteById(string id);
        Task<List<T>> RetrieveAll();
        Task<T> RetrieveById(string id);
    }
}