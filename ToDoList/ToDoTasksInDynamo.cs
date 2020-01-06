using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using ConsoleApp3;

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

        public ToDoTask Update(ToDoTask toDoTask)
        {
            throw new System.NotImplementedException();
        }

        public ToDoTask DeleteById(int taskId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<ToDoTask>> RetrieveAll()
        {
            var request = new ScanRequest
            {
                TableName = TableName
            };

            var foo = await _dbClient.ScanAsync(request);
            var searchResults = foo.Items;

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