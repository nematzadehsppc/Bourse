using Back.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BourseApi.Contract
{
    public interface IItemContract
    {
        void Add(Item item);

        IEnumerable<Item> GetAll();

        Item Find(int key);

        Item Remove(int key);

        void Update(Item item);

        int GetIdByName(string name);
    }
}
