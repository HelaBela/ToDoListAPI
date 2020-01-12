using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoAPI
{
    public class ItemsInMemoryRepository : IRepository<Item>
    {
        private readonly Dictionary<string, Item> _itemsDataBase = new Dictionary<string, Item>();

        public async Task<Item> Create(Item entity)
        {
             _itemsDataBase.Add(entity.Id, entity);

            return entity;
        }

        public  Task<Item> Update(Item entity)
        {
            _itemsDataBase[entity.Id] = entity;

            return Task.FromResult(entity);
        }

        public async Task DeleteById(string id)
        {
            _itemsDataBase.Remove(id);
        }

        public Task<List<Item>> RetrieveAll()
        {
            return Task.FromResult(_itemsDataBase.Values.ToList());
        }

        public Task<Item> RetrieveById(string id)
        {
            return Task.FromResult(_itemsDataBase[id]);
        }

        public Task<bool> IsItemIdInDataBase(string id)
        {
            var itemsStored = _itemsDataBase.Values.ToList();
            var givenItem = itemsStored.Find(s => s.Id == id);

            return Task.FromResult(givenItem != null);
        }
    }
}