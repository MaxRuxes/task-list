using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using TaskList.BLL.DTO;
using TaskList.BLL.Interfaces;
using TaskList.DAL.Entities;
using TaskList.DAL.Interfaces;

namespace TaskList.BLL.Services
{
    public class ProjectsService : IProjectsService
    {
        private readonly IUnitOfWork _database;
        private readonly IMapper _mapper;

        public ProjectsService(IUnitOfWork uow)
        {
            _database = uow;
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDTO, User>();
                cfg.CreateMap<User, UserDTO>();
            }).CreateMapper();
        }

        public IEnumerable<UserDTO> GetAllUsers()
        {
            var list = _database.Users.GetAll();
            return _mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(list);
        }

        public IEnumerable<UserDTO> GetAllUserForCurrentProject(int idProject)
        {
            var usersId = _database.Projects.Find(x => x.IdProjectInfo == idProject)
                .Select(x => x.IdUser);

            var list = _database.Users.Find(x => usersId.Contains(x.UserId));
            return _mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(list);
        }

        public void RemoveUserForProject(int idProject, int idUser)
        {
            var tempRecords = _database.Projects
                .Find(x => x.IdProjectInfo == idProject && x.IdUser == idUser);

            foreach (var record in tempRecords)
            {
                _database.Projects.Delete(record.ProjectsId);
            }

            _database.Save();
        }

        public void AddUserForProject(int idProject, int idUser)
        {
            if (_database.Projects.Find(x => x.IdProjectInfo == idProject && idUser == x.IdUser).Count() != 0)
            {
                return;
            }

            _database.Projects.Create(new Projects()
            {
                IdProjectInfo = idProject,
                IdUser = idUser
            });

            _database.Save();
        }
    }
}
