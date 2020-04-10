//This Sample Code is provided for the purpose of illustration only and is not
//    intended to be used in a production environment.THIS SAMPLE CODE AND ANY
//   RELATED INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//	EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED

//    WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//We grant You a nonexclusive, royalty-free right to use and modify the Sample

//    Code and to reproduce and distribute the object code form of the Sample

//    Code, provided that You agree: 

//    (i) to not use Our name, logo, or trademarks to market Your software

//        product in which the Sample Code is embedded;
//(ii) to include a valid copyright notice on Your software product in 
//		which the Sample Code is embedded; and
//    (iii) to indemnify, hold harmless, and defend Us and Our suppliers from
//        and against any claims or lawsuits, including attorneys fees, that
//        arise or result from the use or distribution of the Sample Code.
//
using System;

namespace RandomDatabaseGenerator
{
    class DataTypes
    {
        #region local variables
        protected string[] dTypes;
        private bool isSQLServer = true;
        #endregion

        // Constructor
        public DataTypes(bool sql)
        {
            isSQLServer = sql;
            Initialize();
        }
        // This method takes a random index and returns the name of the
        // datatype along with the NOT NULL clause
        public string GetDataTypeString(int index)
        {
            string ret = dTypes[index] + " NOT NULL, ";
            return ret;
        }
        // This methoda takes a random index and returns a representative value
        // for the specified datatype
        public string GetDataTypeValue(int index)
        {
            string ret = "";
            switch (index)
            {
                case 0:
                    ret = "0x0f0203040500607080900a0b0c";
                    break;
                case 1:
                    ret = "0x0f0203040500607080900a0b0c";
                    break;
                case 2:
                    ret = "'" + DateTime.Today.ToShortDateString() + "'";
                    break;
                case 3:
                    ret = "'" + DateTime.Today.ToShortTimeString() + "'";
                    break;
                case 4:
                    ret = "3.14";
                    break;
                case 5:
                    ret = Math.Sqrt(2).ToString();
                    break;
                case 6:
                    ret = "3.14159265";
                    break;
                case 7:
                    ret = "42";
                    break;
                case 8:
                    ret = "65535";
                    break;
                case 9:
                    ret = "'ABCDEFGHIJ'";
                    break;
                case 10:
                    ret = "'ABCDEFGHIJABCDEFGHIJABCDEFGHIJABCDEFGHIJABCDEFGHIJ'";
                    break;
                case 11:
                    ret = "N'ABCDEFGHIJ'";
                    break;
                case 12:
                    ret = "N'ABCDEFGHIJABCDEFGHIJABCDEFGHIJABCDEFGHIJABCDEFGHIJ'";
                    break;
                case 13:
                    ret = "'" + DateTime.Today.ToUniversalTime().ToString() + "'";
                    break;
            }
            return ret;
        }
        // initialize our array of available data types
        // several are predicated on whether or not this is SQL Server or other OSS DBs
        public void Initialize()
        {
          dTypes  = new string[14];
            dTypes[0] = "BINARY(50)";
            dTypes[1] = "VARBINARY(50)";
            dTypes[2] = "DATE";
            dTypes[3] = "TIME";
            dTypes[4] = "DECIMAL";
            dTypes[5] = "REAL";
            dTypes[6] = "FLOAT";
            dTypes[7] = "SMALLINT";
            dTypes[8] = "INTEGER";
            dTypes[9] = "CHAR(10)";
            dTypes[10] = isSQLServer ? "VARCHAR(50)" : "VARCHAR2(50)";
            dTypes[11] = "NCHAR(10)";
            dTypes[12] = isSQLServer ? "NVARCHAR(50)" : "NVARCHAR2(50)";
            dTypes[13] = isSQLServer ? "DATETIME" : "TIMESTAMP";
        }
    }
}
