﻿using System.Linq;
using SimpleApp.Core.Interfaces.Repositories;
using SimpleApp.Core.Models.Entities;
using SimpleApp.Infrastructure.Data;

namespace SimpleApp.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext dataContext)
            : base(dataContext)
        {
        }

        public bool CheckIfUserExists(string email)
        {
            return Context.Users.Any(u => u.Email == email);
        }

        public User GetUserByEmail(string email)
        {
            return Context.Users.FirstOrDefault(x => x.Email == email);
        }
    }
}
