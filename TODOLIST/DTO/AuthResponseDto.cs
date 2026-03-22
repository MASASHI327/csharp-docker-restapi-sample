using System.Text.Json.Serialization;

namespace TODOLIST.DTO
{
    public class AuthResponseDto
    {
        [JsonPropertyName("user")]
        public UserDto? USER_TYPE { get; set; } 

        [JsonPropertyName("accessToken")]
        public string ACCESSTOKEN { get; set; } = string.Empty;
        [JsonPropertyName("code")]
        public int CODE { get; set; }

    }
}
