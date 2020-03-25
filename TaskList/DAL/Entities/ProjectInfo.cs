using System.ComponentModel.DataAnnotations.Schema;

namespace TaskList.DAL.Entities
{
    [Table("project")]
    public class ProjectInfo
    {
        [Column("idProject")]
        public int ProjectInfoId { get; set; }

        [Column("name")]
        public string NameProject { get; set; }

        [Column("stackTechnology")]
        public string StackTecnology { get; set; }
    }
}
