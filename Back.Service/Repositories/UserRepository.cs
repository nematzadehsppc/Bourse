using Back.DAL.Context;
using Back.DAL.Models;
using BourseApi.Contract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BourseApi.Repositories
{
    public class UserRepository : IUserContract
    {
        private UAppContext _dbContext;

        public UserRepository(UAppContext context)
        {
            _dbContext = context;
        }

        public IEnumerable<User> GetAll()
        {         
            return _dbContext.Users;
        }

        public void Add(User item)
        {            
            _dbContext.Users.Add(item);
            _dbContext.SaveChanges();
        }

        public User Find(int key)
        {
            return _dbContext.Users.FirstOrDefault(user => user.Id == key);
        }

        public User Remove(int key)
        {
            User item;
            item = _dbContext.Users.Single(user => user.Id == key);
            _dbContext.Users.Remove(item);
            _dbContext.SaveChanges();
            return item;
        }
        
        public void Update(User item)
        {
            _dbContext.Users.Update(item);
            _dbContext.SaveChanges();
        }
    }
}
