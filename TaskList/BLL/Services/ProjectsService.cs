using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            return _mapper.Map<IEnumerable<User>, List<UserDTO>>(list);
        }

        public IEnumerable<UserDTO> GetAllUserForCurrentProject(int idProject)
        {
            var usersID = _database.Projects.Find(x => x.IdProjectInfo == idProject)
                .Select(x => x.IdUser);
            return null;

        }

        public void RemoveUserForProject(int idProject, int idUser)
        {
            throw new NotImplementedException();
        }

        public void AddUserForProject(int idProject, int idUser)
        {
            throw new NotImplementedException();
        }
    }
}
