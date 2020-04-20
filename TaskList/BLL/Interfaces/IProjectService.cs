using System.Collections.Generic;
using TaskList.BLL.DTO;

namespace TaskList.BLL.Interfaces
{
    public interface IProjectService
    {
        IEnumerable<ProjectInfoDTO> GetAllProjects();
        ProjectInfoDTO CreateProject(ProjectInfoDTO project);
        void UpdateProject(ProjectInfoDTO projectInfo);
        void DeleteProject(int idProject);

        int GetCostForProject(int idProject);
        int GetCountWorkersForProject(int idProject);
        int GetCountTodoForProject(int idProject);
        int GetSpentTimeTodoForProject(int idProject);

    }
}
