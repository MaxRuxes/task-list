using AutoMapper;
using System.Collections.Generic;
using TaskList.BLL.DTO;
using TaskList.BLL.Interfaces;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Entities;

namespace TaskList.BLL.Services
{
    public class UserService : IUserService
    {
        private IUnitOfWork DataBase { get; set; }
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork uow)
        {
            DataBase = uow;
            _mapper = new MapperConfiguration(cfg=>
            {
                cfg.CreateMap<User, UserDTO>();
            }).CreateMapper();
        }

        public IEnumerable<UserDTO> GetAllUsers()
        {
            return _mapper.Map<IEnumerable<User>, List<UserDTO>>(DataBase.Users.GetAll());
        }

        public UserDTO GetUser(int? id)
        {
            return _mapper.Map<User, UserDTO>(DataBase.Users.Get(id ?? 1));
        }
    }
}
