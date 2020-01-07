using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDoAPI
{
    public class UserRepository:  IRepository<User>
    {
        public Task<User> Create(User entity)
        {
            throw new System.NotImplementedException();
        }

        public Task<User> Update(User entity)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteById(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<User>> RetrieveAll()
        {
            throw new System.NotImplementedException();
        }

        public User RetrieveById(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}