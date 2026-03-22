using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TODOLIST.Domain
{
    public class TodoEntity 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TODO_ID { get; set; }
        public int USER_ID { get; set; }
        public string? TITLE { get; set; } = string.Empty;
        public string? CONTENT { get; set; } = string.Empty;
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; } 
    }
}
