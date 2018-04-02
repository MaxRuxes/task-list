using AutoMapper;
using System.Linq;
using System.Collections.Generic;
using TaskList.BLL.DTO;
using TaskList.BLL.Interfaces;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Models;

namespace TaskList.BLL.Services
{
    public class TeamService : ITeamService
    {
        private IUnitOfWork Database { get; set; }
        private IMapper mapper;

        public TeamService(IUnitOfWork uow)
        {
            Database = uow;
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TeamInfoDTO, TeamInfo>();
                cfg.CreateMap<TeamInfo, TeamInfoDTO>();
            }).CreateMapper();
        }

        public IEnumerable<TeamInfoDTO> GetAllTeamInfo()
        {
            return mapper.Map<IEnumerable<TeamInfo>, List<TeamInfoDTO>>(Database.TeamInfos.GetAll());
        }

        public TeamInfoDTO GetTeamInfo(int? id)
        {
            return mapper.Map<TeamInfo, TeamInfoDTO>(Database.TeamInfos.Get(id ?? 1));
        }

        public IEnumerable<TeamInfoDTO> GetTeamsForUser(int userId)
        {
            return mapper.Map<IEnumerable<TeamInfo>, List<TeamInfoDTO>>(Database.Teams.Find(o => o.IdUser == userId).Select(o => o.TeamInfo)); 
        }
    }
}
