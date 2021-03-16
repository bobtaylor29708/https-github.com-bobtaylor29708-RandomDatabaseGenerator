using System;
using System.Globalization;

namespace RandomDatabaseGenerator
{
    class oracle : rdbms
    {
        public oracle(string scriptFilePath, string fileName) : base(scriptFilePath, fileName)
        {
            cols.Add("RAW(50)");
            cols.Add("RAW(50)");
            cols.Add("DATE");
            cols.Add("DATE");
            cols.Add("NUMBER");
            cols.Add("DECIMAL");
            cols.Add("REAL");
            cols.Add("NUMBER");
            cols.Add("NUMBER");
            cols.Add("CHAR(10)");
            cols.Add("VARCHAR2(50)");
            cols.Add("NCHAR(10)");
            cols.Add("NVARCHAR2(50)");
            cols.Add("DATE");
            cols.Add("BIGINT");
            vals.Add("'0f0203040500607080900a0b0c'");
            vals.Add("'0f0203040500607080900a0b0c'");
            vals.Add("'" + DateTime.Today.ToString("dd-MMM-yy") + "'");
            vals.Add("'" + DateTime.Today.ToString("dd-MMM-yy") + "'");
            vals.Add("3.14");
            vals.Add(Math.Sqrt(2).ToString());
            vals.Add("3.14159265");
            vals.Add("42");
            vals.Add("65535");
            vals.Add("'ABCDEFGHIJ'");
            vals.Add("'ABCDEFGHIJABCDEFGHIJABCDEFGHIJABCDEFGHIJABCDEFGHIJ'");
            vals.Add("N'ABCDEFGHIJ'");
            vals.Add("N'ABCDEFGHIJABCDEFGHIJABCDEFGHIJABCDEFGHIJABCDEFGHIJ'");
            vals.Add("'" + DateTime.Today.ToString("dd-MMM-yy") + "'");
            vals.Add("400000000");
        }
    }
}
