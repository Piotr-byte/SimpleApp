﻿using SimpleApp.Core.Models.Entities;

namespace SimpleApp.Core.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        public bool CheckIfUserExists(string email);

        public User GetUserByEmail(string email);
    }
}
