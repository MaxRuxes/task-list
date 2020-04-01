using System.ComponentModel;
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

        [Column("isAgile", TypeName = "bit")]
        [DefaultValue(true)]
        public bool IsAgile { get; set; }
    }
}
