using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskList.DAL.Models
{
    [Table("user")]
    public class User
    {
        [Key]
        [Column("idUser")]
        public int IdUser { get; set; }

        [Key]
        [Column("role")]
        public int IdRole { get; set; }

        [Key]
        [Column("fullName")]
        public string FullName { get; set; }

        [Key]
        [Column("telegramContact")]
        public string TelegramContact { get; set; }
    }
}
