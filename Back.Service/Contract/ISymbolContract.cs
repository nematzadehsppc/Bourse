using Back.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BourseApi.Contract
{
    public interface ISymbolContract
    {
        void Add(Symbol item);

        IEnumerable<Symbol> GetAll();

        Symbol Find(int key);

        Symbol Remove(int key);

        void Update(Symbol item);
    }
}
