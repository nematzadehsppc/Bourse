using Back.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BourseApi.Contract
{
    public interface IParamTypeRepository
    {
        void Add(ParamType item);

        IEnumerable<ParamType> GetAll();

        ParamType Find(int key);

        ParamType Remove(int key);

        void Update(ParamType item);

        int GetIdByName(string name);
    }
}
