using System.ComponentModel.DataAnnotations.Schema;

namespace TaskList.DAL.Models
{
    [Table("team")]
    public class TeamInfo
    {
        [Column("idTeam")]
        public int TeamInfoId { get; set; }

        [Column("name")]
        public string NameTeam { get; set; }

        [Column("location")]
        public string Location { get; set; }

        [Column("stackTechnology")]
        public string StackTecnology { get; set; }
    }
}
