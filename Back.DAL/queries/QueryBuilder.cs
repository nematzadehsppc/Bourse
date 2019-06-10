using System;
using System.Collections.Generic;
using System.Text;

namespace Back.DAL.queries
{
    class QueryBuilder
    {
        public const string spInsertParamValue = "CREATE PROCEDURE [dbo].[UD__InsertParamValue__] @pCurrentDate DATETIME, @pValue DECIMAL(18, 2), @pSymbolName NVARCHAR(255), @pItemName NVARCHAR(255) AS  " +
            "BEGIN  " +
            "DECLARE @Value DECIMAL(18, 2) = @pValue  " +
            "DECLARE @SymbolId INT = (SELECT Id FROM __Symbol__ WHERE Name = CAST(@pSymbolName AS NVARCHAR(255)))  " +
            "DECLARE @ItemId INT = (SELECT Id FROM __Item__ WHERE Name = CAST(@pItemName AS NVARCHAR(255)))  " +
            "DECLARE @Count INT= (SELECT COALESCE((SELECT COUNT(*) FROM __ParamValue__  " +
            "WHERE TradingDate = @pCurrentDate AND SymbolId = @SymbolId AND ItemId = @ItemId), 0))  " +
            "IF @Count = 0  " +
            "BEGIN  " +
            "IF @pItemName = N'ميانگين حجم معاملات'  " +
            "BEGIN  " +
            "DECLARE @Avrage DECIMAL(18, 2) = (SELECT dbo.UD__AvrageTransactionVolume__(@pCurrentDate, @SymbolId, @ItemId) ) " +
            "DECLARE @CountOfTransaction DECIMAL(18, 2) = (SELECT COALESCE((SELECT Value FROM __ParamValue__ WHERE SymbolId = @SymbolId AND " +
            "ItemId = (SELECT Id FROM __Item__ WHERE Name = N'حجم معامله') AND TradingDate = @pCurrentDate), 0))  " +
            "IF @Avrage = 0 " +
            "SET @Avrage = @CountOfTransaction " +
            "SET @Value = @CountOfTransaction / @Avrage " +
            "END " +
            "INSERT INTO __ParamValue__(TradingDate, Value, SymbolId, ItemId) VALUES(@pCurrentDate, @Value, @SymbolId, @ItemId) " +
            "END " +
            "END";

        public const string funcAvrageTransactionVolume = "CREATE FUNCTION [dbo].[UD__AvrageTransactionVolume__] (@pCurrentDate DATETIME, @pSymbolId INT, @pItemId INT) " +
            "RETURNS FLOAT " +
            "AS " +
            "BEGIN " +
            "DECLARE @CurrentDate AS DATETIME = DATEADD(DAY, -1, @pCurrentDate) " +
            "DECLARE @DayCount INT = 10 " +
            "DECLARE @Counter INT = 0 " +
            "DECLARE @Value FLOAT = 0.0 " +
            "WHILE(@DayCount<> 0 AND @Counter < 2000) " +
            "BEGIN " +
            "IF((SELECT COALESCE((SELECT 1 FROM __ParamValue__ WHERE SymbolId = @pSymbolId AND ItemId = @pItemId AND TradingDate = @CurrentDate),0)) <> 0) " +
            "BEGIN " +
            "SET @Value = @Value + (SELECT COALESCE((SELECT Value FROM __ParamValue__ WHERE SymbolId = @pSymbolId AND ItemId = @pItemId AND TradingDate = @CurrentDate),0)) " +
            "SET @DayCount = @DayCount - 1 " +
            "END " +
            "SET @CurrentDate = DATEADD(DAY, -1, @CurrentDate) " +
            "SET @Counter = @Counter + 1 " +
            "END " +
            "RETURN @Value / 10 " +
            "END";
    }
}
