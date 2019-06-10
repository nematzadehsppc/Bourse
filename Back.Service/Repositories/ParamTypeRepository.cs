using Back.DAL.Context;
using Back.DAL.Models;
using BourseApi.Contract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace BourseApi.Repositories
{
    public class ParamTypeRepository : IParamTypeRepository
    {
        private UAppContext _dbContext;

        public ParamTypeRepository(UAppContext context)
        {
            _dbContext = context;
        }

        public IEnumerable<ParamType> GetAll()
        {         
            return _dbContext.ParamTypes;
        }

        public void Add(ParamType item)
        {
            _dbContext.ParamTypes.Add(item);
            _dbContext.SaveChanges();
        }

        public ParamType Find(int key) => _dbContext.ParamTypes.Single(param => param.Id == key);

        public ParamType Remove(int key)
        {
            ParamType item;
            item = _dbContext.ParamTypes.Single(param => param.Id == key);
            _dbContext.ParamTypes.Remove(item);
            _dbContext.SaveChanges();
            return item;
        }
        
        public void Update(ParamType item)
        {
            _dbContext.ParamTypes.Update(item);
            _dbContext.SaveChanges();
        }

        public void insertIfNotExist(string value)
        {
            var paramType = _dbContext.ParamTypes.Where(param => param.Name == value)
                            .Select(param => new { param.Id }).FirstOrDefault();

            if (paramType == null || paramType.Id == 0)
                _dbContext.Database.ExecuteSqlCommand("__InsertParamType__ @p0", parameters: new[] { value });
        }

        public int GetIdByName(string name)
        {
            var parameter = new SqlParameter("@pName", name);
            var result = _dbContext.ParamTypes.FromSql("Select Id from __ParamValue__ where Name = @pName", parameter).ToString();
            return Int32.Parse(result); 
        }
    }
}
