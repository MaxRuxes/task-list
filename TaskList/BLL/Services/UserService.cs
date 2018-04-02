using AutoMapper;
using System.Collections.Generic;
using TaskList.BLL.DTO;
using TaskList.BLL.Interfaces;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Models;

namespace TaskList.BLL.Services
{
    public class UserService : IUserService
    {
        private IUnitOfWork DataBase { get; set; }
        private IMapper mapper;

        public UserService(IUnitOfWork uow)
        {
            DataBase = uow;
            mapper = new MapperConfiguration(cfg=>
            {
                cfg.CreateMap<User, UserDTO>().ForMember(x=>x.Role, x=> x.MapFrom(m=>m.Role.NameRole));
                cfg.CreateMap<RolesType, RolesTypeDTO>();
            }).CreateMapper();
        }

        public IEnumerable<UserDTO> GetAllUsers()
        {
            return mapper.Map<IEnumerable<User>, List<UserDTO>>(DataBase.Users.GetAll());
        }

        public UserDTO GetUser(int? id)
        {
            return mapper.Map<User, UserDTO>(DataBase.Users.Get(id ?? 1));
        }
    }
}
