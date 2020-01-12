using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoAPI
{
    public class UserInMemoryRepository : IRepository<User>
    {
        private readonly Dictionary<string, User> _usersDatabase = new Dictionary<string, User>();

        public async Task<User> Create(User entity)
        {
            _usersDatabase.Add(entity.Id, entity);

            return entity;
        }

        public async Task<User> Update(User entity)
        {
            _usersDatabase[entity.Id] = entity;

            return entity;
        }

        public async Task DeleteById(string id)
        {
            _usersDatabase.Remove(id);
        }

        public Task<List<User>> RetrieveAll()
        {
            return Task.FromResult(_usersDatabase.Values.ToList());
        }

        public Task<User> RetrieveById(string id)
        {
            return Task.FromResult(_usersDatabase[id]);
        }

        public Task<bool> IsItemIdInDataBase(string id)
        {
            var itemsStored = _usersDatabase.Values.ToList();
            var givenItem = itemsStored.Find(s => s.Id == id);

            return Task.FromResult(givenItem != null);
        }
    }
}