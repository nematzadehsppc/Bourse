using Back.DAL.Context;
using Back.DAL.Models;
using BourseApi.Contract;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System;
using System.Globalization;
using ClosedXML.Excel;

namespace BourseApi.Repositories
{
    public class ParamValueRepository : IParamValueRepository
    {
        private UAppContext _dbContext;

        public ParamValueRepository(UAppContext context)
        {
            _dbContext = context;
        }

        public IEnumerable<ParamValue> GetAll()
        {
            return _dbContext.ParamValues;
        }

        public void Add(ParamValue item)
        {
            _dbContext.ParamValues.Add(item);
            _dbContext.SaveChanges();
        }

        public ParamValue Find(int key) => _dbContext.ParamValues.Single(param => param.Id == key);

        public ParamValue Remove(int key)
        {
            ParamValue item;
            item = _dbContext.ParamValues.Single(param => param.Id == key);
            _dbContext.ParamValues.Remove(item);
            _dbContext.SaveChanges();
            return item;
        }

        public void Update(ParamValue item)
        {
            _dbContext.ParamValues.Update(item);
            _dbContext.SaveChanges();
        }

        public bool readExcelFile(DataSet dataSet)
        {
            try
            {
                float countOfTransaction = 0;

                for (int rowCount = 0; rowCount < dataSet.Tables[0].Rows.Count; rowCount++)
                {
                    for (int columnCount = 0; columnCount < dataSet.Tables[0].Columns.Count; columnCount++)
                    {
                        DataRow dataRow = dataSet.Tables[0].Rows[rowCount];
                        DataColumn dataColumn = dataSet.Tables[0].Columns[columnCount];
                        string value = dataRow[dataColumn.ColumnName].ToString();

                        if (rowCount == 0 && columnCount != 0)
                        {
                            Item Item = _dbContext.Items.Where(x => x.Name == value).FirstOrDefault();
                            if (Item == null)
                            {
                                Item = new Item();
                                Item.Name = value;
                                _dbContext.Items.Add(Item);
                                _dbContext.SaveChanges();
                            }
                        }

                        // ذخیره کردن نام نماد در صورت عدم وجود در پایگاه داده
                        if (rowCount != 0 && columnCount == 0)
                        {
                            Symbol symbol = _dbContext.Symbols.Where(x => x.Name == value).FirstOrDefault();
                            if (symbol == null)
                            {
                                symbol = new Symbol();
                                symbol.Name = value;
                                _dbContext.Symbols.Add(symbol);
                                _dbContext.SaveChanges();
                            }
                        }

                        // تمام سطر ها و ستون ها جز عنوان سطر 0 ستون 0 (نماد)
                        if (rowCount != 0 && columnCount != 0)
                        {
                            int symbolId = 0, ItemId = 0;
                            string date, ptName, symbolName;

                            date = DateTime.Today.ToString("yyyy-MM-dd");
                            ptName = dataSet.Tables[0].Rows[0][dataSet.Tables[0].Columns[columnCount]].ToString();
                            ItemId = _dbContext.Items.Where(x => x.Name == ptName).FirstOrDefault().Id;

                            symbolName = dataSet.Tables[0].Rows[rowCount][dataSet.Tables[0].Columns[0]].ToString();
                            symbolId = _dbContext.Symbols.Where(x => x.Name == symbolName).FirstOrDefault().Id;

                            ParamValue paramValue = _dbContext.ParamValues.Where(x => x.TradingDate == Convert.ToDateTime(date) && x.SymbolId == symbolId && x.ItemId == ItemId).FirstOrDefault();

                            if (paramValue == null)
                            {
                                if (ItemId != 0 && symbolId != 0)
                                {
                                    paramValue = new ParamValue();
                                    paramValue.TradingDate = DateTime.Today;

                                    if (ptName == "میانگین حجم معاملات" || columnCount == 18)
                                    {
                                        float avrage = GetAvrageTransactionVolume(string.Format("SELECT dbo.UD__AvrageTransactionVolume__ ('{0:yyyy-MM-dd}', {1}, {2})", DateTime.Today.AddDays(1).ToString("yyyy-MM-dd"), symbolId, ItemId));

                                        if (avrage == 0)
                                            avrage = countOfTransaction;

                                        paramValue.Value = countOfTransaction / avrage;
                                    }
                                    else
                                    {
                                        paramValue.Value = (float)(Math.Round(float.Parse(value.ToString()), 2));
                                        if (ptName == "حجم معامله" || columnCount == 12)
                                        {
                                            countOfTransaction = (float)paramValue.Value;
                                        }
                                    }

                                    paramValue.SymbolId = symbolId;
                                    paramValue.ItemId = ItemId;
                                    _dbContext.ParamValues.Add(paramValue);
                                    _dbContext.SaveChanges();
                                }
                            }

                        }

                        Debug.WriteLine("File", dataRow[dataColumn.ColumnName].ToString());
                    }

                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("File", ex.Message.ToString());
                return false;
            }
        }


        public bool readExcelFile(IXLWorksheet workSheet)
        {
            try
            {
                //float countOfTransaction = 0;

                //foreach (IXLRow row in workSheet.Rows())
                //{
                //    foreach (IXLCell cell in row.Cells())
                //    {
                //        string cellValue = cell.Value.ToString();
                //        string ptName = workSheet.Row(1).ToString();
                //    }
                //}

                const int FIRST_ROW = 1;
                const int FIRST_COLUMN = 1;

                //const int VOLUME_OF_TANSACTION_COLUMN = 12;
                //const string VOLUME_OF_TANSACTION_COLUMN_NAME = "حجم معامله";

                //const int COUNT_OF_TANSACTION_COLUMN = 18;
                //const string COUNT_OF_TANSACTION_COLUMN_NAME = "میانگین حجم معاملات";

                int rowCount = workSheet.LastRowUsed().RowNumber();
                int columnCount = workSheet.LastColumnUsed().ColumnNumber();

                //float countOfTransaction = 0;
                for (int rowNumber = FIRST_ROW; rowNumber <= rowCount; rowNumber++)
                {
                    IXLRow row = workSheet.Row(rowNumber);
                    for (int columnNumber = FIRST_COLUMN; columnNumber <= columnCount; columnNumber++)
                    {
                        IXLCell cell = row.Cell(columnNumber);
                        string cellValue = cell.Value.ToString();

                        if (rowNumber == FIRST_ROW && columnNumber != FIRST_COLUMN)
                        {
                            string itemName = cellValue.Replace("ي", "ی").Replace("ك", "ک");
                            Item Item = _dbContext.Items.Where(x => x.Name == itemName).FirstOrDefault();
                            if (Item == null)
                            {
                                Item = new Item();
                                Item.Name = itemName;
                                _dbContext.Items.Add(Item);
                                _dbContext.SaveChanges();
                            }
                        }

                        else if (rowNumber != FIRST_ROW)
                        {
                            if (columnNumber == FIRST_COLUMN)
                            {
                                string symbolName = cellValue.Replace("ي", "ی").Replace("ك", "ک");
                                Symbol symbol = _dbContext.Symbols.Where(x => x.Name == symbolName).FirstOrDefault();
                                if (symbol == null)
                                {
                                    symbol = new Symbol();
                                    symbol.Name = symbolName;
                                    _dbContext.Symbols.Add(symbol);
                                    _dbContext.SaveChanges();
                                }
                            }
                            else
                            {
                                DateTime spDate = DateTime.ParseExact("2019-04-30 00:00:00.000", "yyyy-MM-dd HH:mm:ss.fff",
                                       CultureInfo.InvariantCulture);

                                var currentDate = new SqlParameter("@pCurrentDate", spDate);
                                var value = new SqlParameter("@pValue", cellValue.ToString() == "" || cellValue.ToString() == "NaN" ? 0 : (float)(Math.Round(float.Parse(cellValue.ToString()), 2)));
                                var symbolName = new SqlParameter("@pSymbolName", row.Cell(FIRST_COLUMN).Value.ToString().Replace("ي", "ی").Replace("ك", "ک"));
                                var itemName = new SqlParameter("@pItemName", workSheet.Row(FIRST_ROW).Cell(columnNumber).Value.ToString().Replace("ي", "ی").Replace("ك", "ک"));
                                _dbContext.Database.ExecuteSqlCommand("EXEC UD__InsertParamValue__ @pCurrentDate , @pValue , @pSymbolName , @pItemName", currentDate, value, symbolName, itemName);
                            }

                        }

                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("File", ex.Message.ToString());
                return false;
            }
        }


        private float GetAvrageTransactionVolume(string query)
        {
            float avrage = 0;

            try
            {
                var conn = _dbContext.Database.GetDbConnection();
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText = query;
                avrage = (float)(Math.Round(float.Parse(command.ExecuteScalar().ToString()), 2));
                conn.Close();

            }
            catch (Exception ex)
            {
                Debug.WriteLine("File", ex.Message.ToString());
            }

            return avrage;
        }
    }
}
