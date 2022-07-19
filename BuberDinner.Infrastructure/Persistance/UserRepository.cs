using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Entities;

namespace BuberDinner.Infrastructure.Persistance
{
    public class UserRepository : IUserRepository
    {
        private static ICollection<User> Users {get; set;} = new List<User>();
        public void Add(User user)
        {
            Users.Add(user);
        }

        public User? GetUserByEmail(string email)
        {
            return Users.FirstOrDefault(u => u.Email == email);
        }

        public bool Exist(string email)
        {
            return Users.Any(u => u.Email == email);
        }
    }
}