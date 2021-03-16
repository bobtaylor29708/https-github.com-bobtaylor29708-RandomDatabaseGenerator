using System;
using System.IO;

namespace RandomDatabaseGenerator
{
    class postgreSQL:rdbms
    {
        public postgreSQL(string scriptFilePath,string fileName) : base(scriptFilePath, fileName)
        {
            cols.Add("BINARY(50)");
            cols.Add("VARBINARY(50)");
            cols.Add("DATE");
            cols.Add("TIME");
            cols.Add("NUMERIC");
            cols.Add("DECIMAL");
            cols.Add("REAL");
            cols.Add("SMALLINT");
            cols.Add("INT");
            cols.Add("CHAR(10)");
            cols.Add("VARCHAR(50)");
            cols.Add("NCHAR(10)");
            cols.Add("NVARCHAR(50)");
            cols.Add("DATETIME");
            cols.Add("BIGINT");
            vals.Add("0x0f0203040500607080900a0b0c");
            vals.Add("0x0f0203040500607080900a0b0c");
            vals.Add("'" + DateTime.Today.ToShortDateString() + "'");
            vals.Add("'" + DateTime.Today.ToShortTimeString() + "'");
            vals.Add("3.14");
            vals.Add(Math.Sqrt(2).ToString());
            vals.Add("3.14159265");
            vals.Add("42");
            vals.Add("65535");
            vals.Add("'ABCDEFGHIJ'");
            vals.Add("'ABCDEFGHIJABCDEFGHIJABCDEFGHIJABCDEFGHIJABCDEFGHIJ'");
            vals.Add("N'ABCDEFGHIJ'");
            vals.Add("N'ABCDEFGHIJABCDEFGHIJABCDEFGHIJABCDEFGHIJABCDEFGHIJ'");
            vals.Add("'" + DateTime.Today.ToUniversalTime().ToString() + "'");
            vals.Add("400000000");
            //scriptFileStream = File.Create(ScriptFolderPath + "MariaDB.sql");
            //CreateFile();
        }
    }
}
