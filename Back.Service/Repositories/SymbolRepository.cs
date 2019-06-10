using Back.DAL.Context;
using Back.DAL.Models;
using BourseApi.Contract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace BourseApi.Repositories
{
    public class SymbolRepository : ISymbolRepository
    {
        private UAppContext _dbContext;

        public SymbolRepository(UAppContext context)
        {
            _dbContext = context;
        }

        public IEnumerable<Symbol> GetAll()
        {         
            return _dbContext.Symbols;
        }

        public void Add(Symbol item)
        {
            _dbContext.Symbols.Add(item);
            //_dbContext.Database.ExecuteSqlCommand("__InsertSymbol__ @p0", parameters: new[] { item.Name });
            _dbContext.SaveChanges();
        }
        
        public Symbol Find(int key) => _dbContext.Symbols.Single(symbol => symbol.Id == key);

        public Symbol Remove(int key)
        {
            Symbol item;
            item = _dbContext.Symbols.Single(user => user.Id == key);
            _dbContext.Symbols.Remove(item);
            _dbContext.SaveChanges();
            return item;
        }

        public void Update(Symbol item)
        {
            _dbContext.Symbols.Update(item);
            _dbContext.SaveChanges();
        }
    }
}
