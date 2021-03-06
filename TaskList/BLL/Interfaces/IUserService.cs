﻿using System.Collections.Generic;
using TaskList.BLL.DTO;

namespace TaskList.BLL.Interfaces
{
    public interface IUserService
    {
        IEnumerable<UserDTO> GetAllUsers();
        UserDTO GetUser(int id);
        void ChangeActiveForUser(int id, bool active);
        void UpdateUser(UserDTO user);
        void CreateUser(UserDTO user);
    }
}
