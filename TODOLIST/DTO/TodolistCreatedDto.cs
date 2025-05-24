namespace TODOLIST.DTO
{
    public class TodolistCreatedDto : IUserIdentifiable
    {
        public int USER_ID { get; set; }
        public string TITLE { get; set; } = string.Empty;
        public string CONTENT { get; set; } = string.Empty;
        public DateTime CREATED_DATE { get; set; }
    }
}
