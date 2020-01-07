using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace ToDoAPI
{
    public class ToDoTasksInDynamo : IToDoTaskListsData
    {
        public ToDoTask Tasks { get; }
        private const string TableName = "HelenaToDoList";
        private readonly AmazonDynamoDBClient _dbClient;

        public ToDoTasksInDynamo()
        {
            _dbClient = new AmazonDynamoDBClient();
        }

        public async Task<ToDoTask> Create(ToDoTask toDoTask)
        {
            var response = await _dbClient.PutItemAsync(TableName,
                new Dictionary<string, AttributeValue>
                {
                    {"Id", new AttributeValue(toDoTask.Id)},
                    {"TaskName", new AttributeValue(toDoTask.TaskName)},
                    {"IsCompleted", new AttributeValue(toDoTask.IsCompleted.ToString())},
                });
            return toDoTask;
        }

        public async Task<ToDoTask> Update(ToDoTask toDoTask)
        {
            var response = await _dbClient.UpdateItemAsync(TableName,
                new Dictionary<string, AttributeValue> {{"Id", new AttributeValue(toDoTask.Id)}},
                new Dictionary<string, AttributeValueUpdate>
                {
                    {"TaskName", new AttributeValueUpdate(new AttributeValue(toDoTask.TaskName), AttributeAction.PUT)},
                    {
                        "IsCompleted",
                        new AttributeValueUpdate(new AttributeValue(toDoTask.IsCompleted.ToString()),
                            AttributeAction.PUT)
                    }
                });
            return toDoTask;
        }

        public async void DeleteById(string taskId)
        {
            var response = await _dbClient.DeleteItemAsync(TableName,
                new Dictionary<string, AttributeValue> {{"Id", new AttributeValue(taskId)}});

            
        }

        public async Task<List<ToDoTask>> RetrieveAll()
        {
            var request = new ScanRequest
            {
                TableName = TableName
            };

            var response = await _dbClient.ScanAsync(request);
            var searchResults = response.Items;

            return ParseItems(searchResults);
        }

        private List<ToDoTask> ParseItems(List<Dictionary<string, AttributeValue>> dataBaseResult)
        {
            var toDoTasks = new List<ToDoTask>();

            foreach (var result in dataBaseResult)
            {
                toDoTasks.Add(ParseItem(result));
            }

            return toDoTasks;
        }

        private ToDoTask ParseItem(Dictionary<string, AttributeValue> item)
        {
            return new ToDoTask(item["Id"].S, item["TaskName"].S, item["IsCompleted"].BOOL);
        }

        public ToDoTask RetrieveById(int taskId)
        {
            throw new System.NotImplementedException();
        }
    }
}