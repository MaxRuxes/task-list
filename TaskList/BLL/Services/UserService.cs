using AutoMapper;
using System.Collections.Generic;
using System.Linq;
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
                cfg.CreateMap<UserDTO, User>();
            }).CreateMapper();
        }

        public IEnumerable<UserDTO> GetAllUsers()
        {
            return _mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(DataBase.Users.GetAll());
        }

        public UserDTO GetUser(int? id)
        {
            return _mapper.Map<User, UserDTO>(DataBase.Users.Get(id ?? 1));
        }

        public void ChangeActiveForUser(int id, bool active)
        {
            var user = DataBase.Users.Find(x => x.UserId == id).FirstOrDefault();
            if (user == null)
            {
                return;
            }

            user.IsActive = active;
            DataBase.Users.Update(user);
            DataBase.Save();
        }

        public void UpdateUser(UserDTO user)
        {
            DataBase.Users.Update(_mapper.Map<UserDTO, User>(user));
            DataBase.Save();
        }

        public void CreateUser(UserDTO user)
        {
            DataBase.Users.Create(_mapper.Map<UserDTO, User>(user));
            DataBase.Save();
        }
    }
}
