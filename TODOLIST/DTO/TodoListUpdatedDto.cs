using System.Text.Json.Serialization;

namespace TODOLIST.DTO
{
    public class TodoListUpdatedDto 
    {
        [JsonPropertyName("id")]
        public int TODOLIST_ID { get; set; }
        [JsonPropertyName("title")]
        public string TITLE { get; set; } = string.Empty;
        [JsonPropertyName("content")]
        public string CONTENT { get; set; } = string.Empty;
        [JsonPropertyName("updated_date")]
        public DateTime? UPDATED_DATE { get; set; }
    }
}
