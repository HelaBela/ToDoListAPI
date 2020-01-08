namespace ToDoAPI
{
    public class Item
    {
        public string TaskId { get; set; }
        public string UserId { get; set; }
        public string TaskName { get; set; }
        public bool IsCompleted { get; set; }

        public Item(string userId, string taskId, string taskName, bool isCompleted)
        {
            UserId = userId;
            TaskId = taskId;
            TaskName = taskName;
            IsCompleted = isCompleted;
        }
    }
}