using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace dev_forum_api.Models
{
    public class Reply
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public int ThreadId { get; set; }

        [Required]
        public int UserId { get; set; }

        [JsonIgnore]
        public User? Author { get; set; }

        [JsonIgnore]
        public ForumThread? Thread { get; set; }
    }    }



