using System.ComponentModel.DataAnnotations.Schema;

namespace TaskList.DAL.Models
{
    [Table("teams")]
    public class Teams
    {
        [Column("idTeams")]
        public int TeamsId { get; set; }

        [Column("idTeam")]
        public int IdTeamInfo { get; set; }

        [Column("idUser")]
        public int IdUser { get; set; }

        public virtual User User { get; set; }

        public virtual TeamInfo TeamInfo { get; set; }
    }
}
