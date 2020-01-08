using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace ToDoAPI
{
    public class UserRepository : IRepository<User>
    {
        private const string TableName = "TodoAppUsers";
        private readonly AmazonDynamoDBClient _dbClient;

        public UserRepository()
        {
            _dbClient = new AmazonDynamoDBClient();
        }

        public async Task<User> Create(User user)
        {
            var response = await _dbClient.PutItemAsync(TableName,
                new Dictionary<string, AttributeValue>
                {
                    {"UserId", new AttributeValue(user.Id)},
                    {"UserName", new AttributeValue(user.UserName)},
                });
            return user;
        }

        public async Task<User> Update(User user)
        {
            var response = await _dbClient.UpdateItemAsync(TableName,
                new Dictionary<string, AttributeValue> {{"Id", new AttributeValue(user.Id)}},
                new Dictionary<string, AttributeValueUpdate>
                {
                    {"UserName", new AttributeValueUpdate(new AttributeValue(user.UserName), AttributeAction.PUT)},
                });
            return user;
        }

        public async Task DeleteById(string userId)
        {
            var response = await _dbClient.DeleteItemAsync(TableName,
                new Dictionary<string, AttributeValue> {{"Id", new AttributeValue(userId)}});
        }

        public async Task<List<User>> RetrieveAll()
        {
            var request = new ScanRequest
            {
                TableName = TableName
            };

            var response = await _dbClient.ScanAsync(request);
            var searchResults = response.Items;

            return ParseItems(searchResults);
        }

        private List<User> ParseItems(List<Dictionary<string, AttributeValue>> dataBaseResult)
        {
            var users = new List<User>();

            foreach (var result in dataBaseResult)
            {
                users.Add(ParseItem(result));
            }

            return users;
        }

        private User ParseItem(Dictionary<string, AttributeValue> item)
        {
            return new User(item["UserId"].S, item["UserName"].S);
        }

        public Task<Item> RetrieveById(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}