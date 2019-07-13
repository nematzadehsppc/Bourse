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
            item.Password = SecurityParameters.MD5Encryption(item.Password);
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

        public Tuple<AuthenticationResult, User> AddUser(string name, string family, string username, string password, string email, DateTime birthday)
        {
            User _user = new User();

            _user = _dbContext.Users.FirstOrDefault(user => user.UserName == username);
            if (_user != null)
                return new Tuple<AuthenticationResult, User>(new AuthenticationResult(AuthenticationResultCode.ClientConnectivityError), null);

            _user = new User();
            _user.Name = name;
            _user.FamilyName = family;
            _user.UserName = username;
            _user.Password = SecurityParameters.MD5Encryption(password);
            _user.Email = email;
            _user.BirthDate = birthday;

            _dbContext.Users.Add(_user);
            _dbContext.SaveChanges();

            return new Tuple<AuthenticationResult, User>(new AuthenticationResult(AuthenticationResultCode.AuthenticationSuccess), _user);
        }
    }
}
