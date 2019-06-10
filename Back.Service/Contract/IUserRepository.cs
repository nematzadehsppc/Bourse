using Back.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BourseApi.Contract
{
    public interface IUserRepository
    {
        void Add(User item);

        IEnumerable<User> GetAll();

        User Find(int key);

        User Remove(int key);

        void Update(User item);
    }
}
