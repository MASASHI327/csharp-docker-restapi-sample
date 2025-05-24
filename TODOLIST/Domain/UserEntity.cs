using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TODOLIST.Domain
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int USER_ID { get; set; }
        public string USER_NAME { get; set; } = string.Empty;
        public string EMAIL_ADDRESS { get; set; } = string.Empty;
        public string PASSWORD { get; set; } = string.Empty;
    }
}
