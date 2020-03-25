using System.Collections.Generic;
using TaskList.BLL.DTO;

namespace TaskList.BLL.Interfaces
{
    public interface IProjectService
    {
        IEnumerable<ProjectInfoDTO> GetTeamsForUser(int userId);
        IEnumerable<ProjectInfoDTO> GetAllTeamInfo();
        ProjectInfoDTO GetTeamInfo(int? id);
    }
}
