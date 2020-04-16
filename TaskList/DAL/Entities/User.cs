using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskList.DAL.Entities
{
    [Table("user")]
    public class User
    {
        [Column("idUser")]
        public int UserId { get; set; }

        [Column("role")]
        public string Role { get; set; }

        [Column("fullName")]
        public string FullName { get; set; }

        [Column("telegramContact")]
        public string TelegramContact { get; set; }

        [Column("isActive", TypeName = "bit")]
        [DefaultValue(true)]
        public bool IsActive { get; set; }
    }
}
