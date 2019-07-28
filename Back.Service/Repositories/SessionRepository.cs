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
    public class SessionRepository : ISessionContract
    {
        private UAppContext _dbContext;

        public SessionRepository(UAppContext context)
        {
            _dbContext = context;
        }

        public IEnumerable<Session> GetAll()
        {
            return _dbContext.Sessions;
        }

        public void Add(Session session)
        {
            _dbContext.Sessions.Add(session);
            _dbContext.SaveChanges();
        }

        public Session Find(int key)
        {
            return _dbContext.Sessions.FirstOrDefault(session => session.Id == key);
        }

        public Session Remove(int key)
        {
            Session item;
            item = _dbContext.Sessions.Single(session => session.Id == key);
            _dbContext.Sessions.Remove(item);
            _dbContext.SaveChanges();
            return item;
        }

        public void Update(Session item)
        {
            _dbContext.Sessions.Update(item);
            _dbContext.SaveChanges();
        }

        public Tuple<AuthenticationResult, Session> AddSession(int userId, byte[] userSession)
        {
            Session _session = new Session();

            _session = _dbContext.Sessions.FirstOrDefault(session => session.UserId == userId);
            if (_session != null)
                return new Tuple<AuthenticationResult, Session>(new AuthenticationResult(AuthenticationResultCode.ClientConnectivityError), null);

            _session = new Session();
            _session.UserId = userId;
            _session.UserSession = userSession;

            _dbContext.Sessions.Add(_session);
            _dbContext.SaveChanges();

            return new Tuple<AuthenticationResult, Session>(new AuthenticationResult(AuthenticationResultCode.AuthenticationSuccess), _session);
        }
    }
}
