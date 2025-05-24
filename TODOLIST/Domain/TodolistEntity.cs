using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TODOLIST.Domain
{
    public class Todolist : IUserIdentifiable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TODOLIST_ID { get; set; }
        public int USER_ID { get; set; }
        public string? TITLE { get; set; } = string.Empty;
        public string? CONTENT { get; set; } = string.Empty;
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; } 
    }
}
