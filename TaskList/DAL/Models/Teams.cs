using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskList.DAL.Models
{
    [Table("teams")]
    public class Teams
    {
        [Key]
        [Column("idTeams")]
        public int IdTeams { get; set; }

        [Column("idTeam")]
        public int IdTeamInfo { get; set; }

        [Column("idUser")]
        public int IdUser { get; set; }
    }
}
