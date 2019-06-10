using Back.DAL.Models;
using ClosedXML.Excel;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace BourseApi.Contract
{
    public interface IParamValueRepository
    {
        void Add(ParamValue item);

        IEnumerable<ParamValue> GetAll();

        ParamValue Find(int key);

        ParamValue Remove(int key);

        void Update(ParamValue item);

        bool readExcelFile(DataSet dataSet);

        bool readExcelFile(IXLWorksheet workSheet);
    }
}
