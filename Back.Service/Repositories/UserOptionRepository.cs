using Back.DAL.Context;
using Back.DAL.Models;
using BourseApi.Contract;
using BourseService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BourseApi.Repositories
{
    public class UserOptionRepository : IUserOptionContract
    {
        private UAppContext _dbContext;

        public UserOptionRepository(UAppContext context)
        {
            _dbContext = context;
        }

        public IEnumerable<UserOption> GetAll()
        {
            return _dbContext.UserOptions;
        }

        public void Add(UserOption userOption)
        {
            _dbContext.UserOptions.Add(userOption);
            _dbContext.SaveChanges();
        }

        public UserOption Find(int id)
        {
            return _dbContext.UserOptions.FirstOrDefault(userOption => userOption.Id == id);
        }

        public UserOption FindByUserId(int userId)
        {
            return _dbContext.UserOptions.FirstOrDefault(userOption => userOption.UserId == userId);
        }

        public UserOption Remove(int key)
        {
            UserOption _userOption;
            _userOption = _dbContext.UserOptions.Single(userOption => userOption.Id == key);
            _dbContext.UserOptions.Remove(_userOption);
            _dbContext.SaveChanges();
            return _userOption;
        }

        public void Update(UserOption item)
        {
            _dbContext.UserOptions.Update(item);
            _dbContext.SaveChanges();
        }
    }
}
