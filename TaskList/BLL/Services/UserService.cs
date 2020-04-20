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
        private readonly IUnitOfWork _dataBase;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork uow)
        {
            _dataBase = uow;
            _mapper = new MapperConfiguration(cfg=>
            {
                cfg.CreateMap<User, UserDTO>();
                cfg.CreateMap<UserDTO, User>();
            }).CreateMapper();
        }

        public IEnumerable<UserDTO> GetAllUsers()
        {
            return _mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(_dataBase.Users.GetAll());
        }

        public UserDTO GetUser(int id)
        {
            return _mapper.Map<User, UserDTO>(_dataBase.Users.Get(id));
        }

        public void ChangeActiveForUser(int id, bool active)
        {
            var user = _dataBase.Users.Find(x => x.UserId == id).FirstOrDefault();
            if (user == null)
            {
                return;
            }

            user.IsActive = active;
            _dataBase.Users.Update(user);
            _dataBase.Save();
        }

        public void UpdateUser(UserDTO user)
        {
            _dataBase.Users.Update(_mapper.Map<UserDTO, User>(user));
            _dataBase.Save();
        }

        public void CreateUser(UserDTO user)
        {
            _dataBase.Users.Create(_mapper.Map<UserDTO, User>(user));
            _dataBase.Save();
        }
    }
}
