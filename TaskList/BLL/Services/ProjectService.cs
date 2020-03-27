using AutoMapper;
using System.Linq;
using System.Collections.Generic;
using TaskList.BLL.DTO;
using TaskList.BLL.Interfaces;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Entities;

namespace TaskList.BLL.Services
{
    public class ProjectService : IProjectService
    {
        private IUnitOfWork Database { get; set; }
        private readonly IMapper mapper;

        public ProjectService(IUnitOfWork uow)
        {
            Database = uow;
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProjectInfoDTO, ProjectInfo>();
                cfg.CreateMap<ProjectInfo, ProjectInfoDTO>();
            }).CreateMapper();
        }

        public IEnumerable<ProjectInfoDTO> GetAllProjects()
        {
            return mapper.Map<IEnumerable<ProjectInfo>, List<ProjectInfoDTO>>(Database.ProjectInfo.GetAll());
        }

        public ProjectInfoDTO GetTeamInfo(int? id)
        {
            return mapper.Map<ProjectInfo, ProjectInfoDTO>(Database.ProjectInfo.Get(id ?? 1));
        }

        public IEnumerable<ProjectInfoDTO> GetProjectsForUser(int userId)
        {
            return mapper.Map<IEnumerable<ProjectInfo>, List<ProjectInfoDTO>>(Database.Projects.Find(o => o.IdUser == userId).Select(o => o.ProjectInfo)); 
        }
    }
}
