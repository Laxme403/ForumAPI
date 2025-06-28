using System.Text.Json.Serialization;

public class UserIdDto
{
    [JsonPropertyName("userId")]
    public int UserId { get; set; }
}

