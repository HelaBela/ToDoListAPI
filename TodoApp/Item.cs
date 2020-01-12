using Newtonsoft.Json;

namespace ToDoAPI
{
    public class Item
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }

        public Item(string userId, string id, string title, bool isCompleted)
        {
            UserId = userId;
            Id = id;
            Title = title;
            IsCompleted = isCompleted;
        }

        public string ConvertToJson()
        {
            return  JsonConvert.SerializeObject(this);
        }

//        public bool IsItemIdValid(string itemId )
//        {
//            if(itemId)
//            
//            return true;
//        }
    }
}