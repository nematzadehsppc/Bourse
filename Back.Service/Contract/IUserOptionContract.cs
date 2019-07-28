using Back.DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BourseApi.Contract
{
    public interface IUserOptionContract
    {
        void Add(UserOption userOption);

        IEnumerable<UserOption> GetAll();

        UserOption Find(int id);

        UserOption FindByUserId(int userId);

        UserOption Remove(int key);

        void Update(UserOption userOption);
    }
}
