using System.ComponentModel.DataAnnotations.Schema;

namespace TaskList.DAL.Entities
{
    [Table("user")]
    public class User
    {
        [Column("idUser")]
        public int UserId { get; set; }

        [ForeignKey("Role")]
        [Column("role")]
        public int IdRole { get; set; }

        [Column("fullName")]
        public string FullName { get; set; }

        [Column("telegramContact")]
        public string TelegramContact { get; set; }

        public virtual RolesType Role { get; set; }
    }
}
