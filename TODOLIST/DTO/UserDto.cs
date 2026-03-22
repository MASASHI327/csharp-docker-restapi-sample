using System.Text.Json.Serialization;

namespace TODOLIST.DTO
{
    public class UserDto
    {
        [JsonPropertyName("id")]
        public int USER_ID { get; set; }
        [JsonPropertyName("name")]
        public string USER_NAME { get; set; } = string.Empty;
        [JsonPropertyName("email")]
        public string EMAIL_ADDRESS { get; set; } = string.Empty;
        [JsonPropertyName("password")]
        public string PASSWORD { get; set; } = string.Empty;
    }
}
