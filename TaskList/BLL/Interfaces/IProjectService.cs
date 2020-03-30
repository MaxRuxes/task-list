﻿using System.Collections.Generic;
using TaskList.BLL.DTO;

namespace TaskList.BLL.Interfaces
{
    public interface IProjectService
    {
        IEnumerable<ProjectInfoDTO> GetProjectsForUser(int userId);
        IEnumerable<ProjectInfoDTO> GetAllProjects();

        ProjectInfoDTO CreateProject(ProjectInfoDTO project);
        void UpdateProject(ProjectInfoDTO projectInfo);
        void DeleteProject(int idProject);

    }
}
