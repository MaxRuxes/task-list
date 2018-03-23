using System.ComponentModel.DataAnnotations.Schema;

namespace TaskList.DAL.Models
{
    [Table("user")]
    public class User
    {
        [Column("idUser")]
        public int UserId { get; set; }

        [Column("role")]
        public int IdRole { get; set; }

        [Column("fullName")]
        public string FullName { get; set; }

        [Column("telegramContact")]
        public string TelegramContact { get; set; }
    }
}
