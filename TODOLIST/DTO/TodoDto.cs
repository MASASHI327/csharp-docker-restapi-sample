using System.Text.Json.Serialization;

namespace TODOLIST.DTO
{
    public class TodoDto
    {
        [JsonPropertyName("id")]
        public int TODO_ID { get; set; }
        [JsonPropertyName("user_id")]
        public int USER_ID { get; set; }
        [JsonPropertyName("title")]
        public string TITLE { get; set; } = string.Empty;
        [JsonPropertyName("content")]
        public string CONTENT { get; set; } = string.Empty;
        [JsonPropertyName("code")]
        public int CODE { get; set; }
    }
}
