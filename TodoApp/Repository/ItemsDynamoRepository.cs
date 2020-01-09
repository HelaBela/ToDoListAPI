using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace ToDoAPI
{
    public class ItemsDynamoRepository : IRepository<Item>
    {
        private const string TableName = "TodoApp";
        private readonly AmazonDynamoDBClient _dbClient;

        public ItemsDynamoRepository()
        {
            _dbClient = new AmazonDynamoDBClient();
        }

        public async Task<Item> Create(Item item)
        {
            var response = await _dbClient.PutItemAsync(TableName,
                new Dictionary<string, AttributeValue>
                {
                    {"UserId", new AttributeValue(item.UserId)},
                    {"TaskId", new AttributeValue(item.Id)},
                    {"TaskName", new AttributeValue(item.Title)},
                    {"IsCompleted", new AttributeValue {BOOL = item.IsCompleted}},
                });
            return item;
        }

        public async Task<Item> Update(Item item)
        {
            var response = await _dbClient.UpdateItemAsync(TableName,
                new Dictionary<string, AttributeValue> {{"Id", new AttributeValue(item.Id)}},
                new Dictionary<string, AttributeValueUpdate>
                {
                    {"TaskName", new AttributeValueUpdate(new AttributeValue(item.Title), AttributeAction.PUT)},
                    
                    {
                        "UserId",
                        new AttributeValueUpdate(new AttributeValue(item.UserId),
                            AttributeAction.PUT)
                    },
                    {
                        "IsCompleted",
                        new AttributeValueUpdate(new AttributeValue {BOOL = item.IsCompleted},
                            AttributeAction.PUT)
                    }
                });
            return item;
        }

        public async Task DeleteById(string taskId)
        {
            var response = await _dbClient.DeleteItemAsync(TableName,
                new Dictionary<string, AttributeValue> {{"TaskId", new AttributeValue(taskId)}});
        }

//        public async Task<List<Item>> RetrieveAllByUserId(string userId)
//        {
//            var request = new GetItemRequest()
//            {
//                TableName = TableName,
//                Key = userId
//            };
//
//            var response = await _dbClient.GetItemAsync(request);
//
//            return ParseItems(response.Item);
//        }

        public async Task<List<Item>> RetrieveAll() // (string userId)
        {
            var request = new ScanRequest
            {
                TableName = TableName
            };

            var response = await _dbClient.ScanAsync(request);
            var searchResults = response.Items;

            return ParseItems(searchResults);
        }

        private List<Item> ParseItems(List<Dictionary<string, AttributeValue>> dataBaseResult)
        {
            var toDoTasks = new List<Item>();

            foreach (var result in dataBaseResult)
            {
                toDoTasks.Add(ParseItem(result));
            }

            return toDoTasks;
        }

        private Item ParseItem(Dictionary<string, AttributeValue> item)
        {
            return new Item(item["UserId"].S,item["TaskId"].S,item["TaskName"].S, item["IsCompleted"].BOOL);
        }

        public Task<Item> RetrieveById(string taskId)
        {
            throw new System.NotImplementedException();
        }
    }
}