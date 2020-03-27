using System.Collections.Generic;
using TaskList.BLL.DTO;

namespace TaskList.BLL.Interfaces
{
    public interface IProjectService
    {
        IEnumerable<ProjectInfoDTO> GetProjectsForUser(int userId);
        IEnumerable<ProjectInfoDTO> GetAllProjects();
        ProjectInfoDTO GetTeamInfo(int? id);
    }
}
