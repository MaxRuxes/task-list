using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskList.DAL.Models
{
    [Table("team")]
    public class TeamInfo
    {
        [Key]
        [Column("idTeam")]
        public int IdTeam { get; set; }

        [Column("name")]
        public string NameTeam { get; set; }

        [Column("location")]
        public string Location { get; set; }

        [Column("stackTechnology")]
        public string StackTecnology { get; set; }
    }
}
