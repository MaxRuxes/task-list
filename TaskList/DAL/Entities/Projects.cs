using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskList.DAL.Entities
{
    [Table("projects")]
    public class Projects
    {
        [Column("idProjects")]
        public int ProjectsId { get; set; }

        [ForeignKey("User")]
        [Column("idUser")]
        public int IdUser { get; set; }

        [ForeignKey("ProjectInfo")]
        [Column("idProject")]
        public int IdProjectInfo { get; set; }

        public virtual User User { get; set; }

        public virtual ProjectInfo ProjectInfo { get; set; }
    }
}
