namespace ToDoAPI
{
    public class ToDoTask
    {
        public string Id { get; set; }
        public string TaskName { get; set; }
        public bool IsCompleted { get; set; }

        public ToDoTask(string id, string taskName, bool isCompleted)
        {
            Id = id;
            TaskName = taskName;
            IsCompleted = isCompleted;
        }
    }
}