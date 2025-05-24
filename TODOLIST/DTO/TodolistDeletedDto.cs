namespace TODOLIST.DTO
{
    public class TodolistDeletedDto : IUserIdentifiable
    {
        public int TODOLIST_ID { get; set; }
        public int USER_ID { get; set; }
    }
}
