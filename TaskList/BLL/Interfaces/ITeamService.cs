using System.Collections.Generic;
using TaskList.BLL.DTO;

namespace TaskList.BLL.Interfaces
{
    public interface ITeamService
    {
        IEnumerable<TeamInfoDTO> GetTeamsForUser(int userId);
        IEnumerable<TeamInfoDTO> GetAllTeamInfo();
        TeamInfoDTO GetTeamInfo(int? id);
    }
}
