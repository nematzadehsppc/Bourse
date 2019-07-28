using Back.DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BourseApi.Contract
{
    public interface ISessionContract
    {
        void Add(Session session);

        Tuple<AuthenticationResult, Session> AddSession(int userId, byte[] userSession);

        IEnumerable<Session> GetAll();

        Session Find(int key);

        Session Remove(int key);

        void Update(Session item);
    }
}
