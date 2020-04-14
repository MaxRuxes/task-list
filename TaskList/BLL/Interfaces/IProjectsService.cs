using System.Collections.Generic;
using TaskList.BLL.DTO;
using TaskList.DAL.Entities;

namespace TaskList.BLL.Interfaces
{
    public interface IProjectsService
    {
        IEnumerable<UserDTO> GetAllUsers();
        IEnumerable<UserDTO> GetAllUserForCurrentProject(int idProject);
        void RemoveUserForProject(int idProject, int idUser);
        void AddUserForProject(int idProject, int idUser);
    }
}