using Back.DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BourseApi.Contract
{
    public interface IUserContract
    {
        void Add(User item);

        Tuple<AuthenticationResult, User> AddUser(string name, string family, string username, string password, string email, DateTime birthday);

        IEnumerable<User> GetAll();

        User Find(int key);

        User Remove(int key);

        void Update(User item);
    }
}
