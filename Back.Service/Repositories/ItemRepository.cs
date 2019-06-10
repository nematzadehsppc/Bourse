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
    public class ItemRepository : IItemRepository
    {
        private UAppContext _dbContext;

        public ItemRepository(UAppContext context)
        {
            _dbContext = context;
        }

        public IEnumerable<Item> GetAll()
        {         
            return _dbContext.Items;
        }

        public void Add(Item item)
        {
            _dbContext.Items.Add(item);
            _dbContext.SaveChanges();
        }

        public Item Find(int key) => _dbContext.Items.Single(param => param.Id == key);

        public Item Remove(int key)
        {
            Item item;
            item = _dbContext.Items.Single(param => param.Id == key);
            _dbContext.Items.Remove(item);
            _dbContext.SaveChanges();
            return item;
        }
        
        public void Update(Item item)
        {
            _dbContext.Items.Update(item);
            _dbContext.SaveChanges();
        }

        public void insertIfNotExist(string value)
        {
            var Item = _dbContext.Items.Where(param => param.Name == value)
                            .Select(param => new { param.Id }).FirstOrDefault();

            if (Item == null || Item.Id == 0)
                _dbContext.Database.ExecuteSqlCommand("__InsertItem__ @p0", parameters: new[] { value });
        }

        public int GetIdByName(string name)
        {
            var parameter = new SqlParameter("@pName", name);
            var result = _dbContext.Items.FromSql("Select Id from __ParamValue__ where Name = @pName", parameter).ToString();
            return Int32.Parse(result); 
        }
    }
}
