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

using System;
using System.Configuration;
using System.IO;
using System.Text;


namespace RandomDatabaseGenerator
{
    class Program
    {
        #region Local variables
        public static bool CreateSQLScript;
        public static bool CreateOracleScript;
        public static bool CreatePostgreSQLScript;
        public static bool CreateMySQLScript;
        public static bool CreateMariaDBScript;
        public static int NumberOfTablesToCreate;
        public static int MaxNumberOfColumnsPerTable;
        public static int MaxNumberOfRowsPerTable;
        public static string DatabaseName;
        public static Random r;

        public static sql sqlServer; 
        public static oracle oracleServer;
        public static postgreSQL postgreSQLServer;
        public static mySQL mySQLServer;
        public static mariaDB mariaDBServer;

        public static string ScriptFolderPath;
        public static FileStream logFile = null;
        #endregion  
        
        static void Main(string[] args)
        {
            // Read app.config to see what we need to do
            GetConfigurationValues();
            // set up our log file to record activities
            logFile = File.Create(ScriptFolderPath + "Activity.log");
            // inialized our Random number generator
            r = new Random();
            DateTime startTime = DateTime.Now;
            WriteInColor(ConsoleColor.Yellow, string.Format("Started at :{0}", startTime),true);
            // For each of the rdbms systems, create their filestream objects
            InitializeFiles();
            // System specific CREATE DATABASE statements
            InitializeDB();
            // Let make tables
            CreateTables();
            Console.WriteLine("Complete");
            DateTime endTime = DateTime.Now;
            WriteInColor(ConsoleColor.Yellow, string.Format("Finished at :{0}", endTime),true);
            WriteInColor(ConsoleColor.Yellow, string.Format("Elapsed Time {0}", endTime - startTime),true);
            WriteInColor(ConsoleColor.Red, "Press any key to end.",true);
            // Make sure all filestream objects have flushed and closed their streams.
            FlushAndCloseFiles();
            Console.ReadKey(true);
        }

        #region Helper Functions
        // Read all configuration items from App.Config and populate local variables
        public static void GetConfigurationValues()
        {
            if (ConfigurationManager.AppSettings["NumberOfTablesToCreate"] != null)
            {
                int value = 1000;
                int.TryParse(ConfigurationManager.AppSettings["NumberOfTablesToCreate"], out value);
                NumberOfTablesToCreate = value;
            }
            if (ConfigurationManager.AppSettings["MaxNumberOfColumnsPerTable"] != null)
            {
                int value = 8;
                int.TryParse(ConfigurationManager.AppSettings["MaxNumberOfColumnsPerTable"], out value);
                MaxNumberOfColumnsPerTable = value;
            }
            if (ConfigurationManager.AppSettings["MaxNumberOfRowsPerTable"] != null)
            {
                int value = 10;
                int.TryParse(ConfigurationManager.AppSettings["MaxNumberOfRowsPerTable"], out value);
                MaxNumberOfRowsPerTable = value;
            }
            if (ConfigurationManager.AppSettings["CreateSQLScript"] != null)
            {
                bool value = false;
                bool.TryParse(ConfigurationManager.AppSettings["CreateSQLScript"], out value);
                CreateSQLScript = value;
            }
            if (ConfigurationManager.AppSettings["CreateOracleScript"] != null)
            {
                bool value = false;
                bool.TryParse(ConfigurationManager.AppSettings["CreateOracleScript"], out value);
                CreateOracleScript = value;
            }
            if (ConfigurationManager.AppSettings["CreatePostgreSQLScript"] != null)
            {
                bool value = false;
                bool.TryParse(ConfigurationManager.AppSettings["CreatePostgreSQLScript"], out value);
                CreatePostgreSQLScript = value;
            }
            if (ConfigurationManager.AppSettings["CreateMySQLScript"] != null)
            {
                bool value = false;
                bool.TryParse(ConfigurationManager.AppSettings["CreateMySQLScript"], out value);
                CreateMySQLScript = value;
            }
            if (ConfigurationManager.AppSettings["CreateMariaDBScript"] != null)
            {
                bool value = false;
                bool.TryParse(ConfigurationManager.AppSettings["CreateMariaDBScript"], out value);
                CreateMariaDBScript = value;
            }
            if (ConfigurationManager.AppSettings["DatabaseName"] != null)
            {
                DatabaseName = ConfigurationManager.AppSettings["DatabaseName"].ToString();
            }
            if (ConfigurationManager.AppSettings["ScriptFolderPath"] != null)
            {
                ScriptFolderPath = ConfigurationManager.AppSettings["ScriptFolderPath"].ToString();
            }
        }
        
        // Writes a message in color to the console. Also duplicates that message 
        // in the activity log
        private static void WriteInColor(ConsoleColor c, string message, bool addReturn = false)
        {
            Console.ForegroundColor = c;
            if (addReturn)
            {
                Console.WriteLine(message);
            }
            else
            {
                Console.Write(message);
            }
            Console.ResetColor();
            WriteToLog(message, addReturn);
        }
        
        // Method to localize all writes to the activity log. When called from WriteInColor 
        // we will not append a cararige return.
        private static void WriteToLog(string message, bool addReturn = false)
        {
            if (logFile != null)
            {
                message += (addReturn) ? "\r\n" : "";
                Byte[] info = new UTF8Encoding(false).GetBytes(message);
                logFile.Write(info, 0, info.Length);
            }
        }
        
        // Method will do initialize the FileStream objects for each system type
        private static void InitializeFiles()
        {
            if (CreateSQLScript == true)
            {
                sqlServer = new sql(ScriptFolderPath, "SQL Server.sql");
            }
            if (CreateOracleScript == true)
            {
                oracleServer = new oracle(ScriptFolderPath, "Oracle.sql");
            }
            if (CreatePostgreSQLScript == true)
            {
                postgreSQLServer = new postgreSQL(ScriptFolderPath, "PostgreSQL.sql");
            }
            if (CreateMySQLScript == true)
            {
                mySQLServer = new mySQL(ScriptFolderPath, "MySQL.sql");
            }
            if (CreateMariaDBScript == true)
            {
                mariaDBServer = new mariaDB(ScriptFolderPath, "MariaDB.sql");
            }
        }

        //TODO: implement InitializedB() for remaining systems.
        // Method will do CREATE DATABASE specific script
        private static void InitializeDB()
        {
            WriteInColor(ConsoleColor.White, "Re-initilizing database", true);
            if (CreateSQLScript)
            {
                sqlServer.InitializeDB(DatabaseName);
            }
            if (CreateOracleScript == true)
            {
                oracleServer.InitializeDB(DatabaseName); ;
            }
            if (CreatePostgreSQLScript == true)
            {
                postgreSQLServer.InitializeDB(DatabaseName);
            }
            if (CreateMySQLScript == true)
            {
                mySQLServer.InitializeDB(DatabaseName);
            }
            if (CreateMariaDBScript == true)
            {
                mariaDBServer.InitializeDB(DatabaseName);
            }
        }

        // Based on the configurations values we will create X tables with a random number
        // of rows up to the maximum specified in App.Config
        private static void CreateTables()
        {
            for (int tables = 0; tables < NumberOfTablesToCreate; tables++)
            {
                Preamble(tables.ToString());
                int randomColumnCount = r.Next(3, MaxNumberOfColumnsPerTable);
                for (int columns = 0; columns < randomColumnCount; columns++)
                {
                    // This depends on the number of base data types we will support
                    // right now there are 15...
                    int index = r.Next(0, 14);
                    AddColumns(columns.ToString(), index);
                }
                TrimEnd(false);
                WriteInColor(ConsoleColor.Green, string.Format("Creating table: {0} with {1} columns.\t", tables.ToString(), randomColumnCount));
                int randomRowCount = r.Next(1, MaxNumberOfRowsPerTable);
                for (int count = 0; count < randomRowCount; count++)
                {
                    AddRows(tables, count);
                }
                Reset();
                WriteInColor(ConsoleColor.Yellow, string.Format("Adding {0} rows.", randomRowCount), true);
            }
        }
        
        // Method will ensure all filestreams are flushed and closed
        private static void FlushAndCloseFiles()
        {
            if (sqlServer != null)
            {
                sqlServer.FlushAndClose();
            }
            if (oracleServer != null)
            {
                oracleServer.FlushAndClose();
            }
            if (postgreSQLServer != null)
            {
                postgreSQLServer.FlushAndClose();
            }
            if (mySQLServer != null)
            {
                mySQLServer.FlushAndClose();
            }
            if (mariaDBServer != null)
            {
                mariaDBServer.FlushAndClose();
            }
            logFile.Flush();
            logFile.Close();
        }

        // Method will start the CREATE TABLE statements
        private static void Preamble(string tables)
        {
            if (CreateSQLScript)
            {
                sqlServer.AddToScript("CREATE TABLE Test" + tables.ToString() + "(id int NOT NULL,");
            }
            if (CreateOracleScript == true)
            {
                oracleServer.AddToScript("CREATE TABLE Test" + tables.ToString() + "(id NUMBER NOT NULL,");
            }
            if (CreatePostgreSQLScript == true)
            {
                postgreSQLServer.AddToScript("CREATE TABLE Test" + tables.ToString() + "(id int NOT NULL,");
            }
            if (CreateMySQLScript == true)
            {
                mySQLServer.AddToScript("CREATE TABLE Test" + tables.ToString() + "(id int NOT NULL,");
            }
            if (CreateMariaDBScript == true)
            {
                mariaDBServer.AddToScript("CREATE TABLE Test" + tables.ToString() + "(id int NOT NULL,");
            }
        }

        // Method will add the sets of columns for each system
        private static void AddColumns(string columns, int index)
        {
            if (CreateSQLScript)
            {
                sqlServer.AddColumn(columns, index);
            }
            if (CreateOracleScript == true)
            {
                oracleServer.AddColumn(columns, index);
            }
            if (CreatePostgreSQLScript == true)
            {
                postgreSQLServer.AddColumn(columns, index);
            }
            if (CreateMySQLScript == true)
            {
                mySQLServer.AddColumn(columns, index);
            }
            if (CreateMariaDBScript == true)
            {
                mariaDBServer.AddColumn(columns, index);
            }
        }

        // Method will add rows of random data
        private static void AddRows(int tables, int count)
        {
            if (sqlServer != null)
            {
                StringBuilder insert = new StringBuilder("Insert into Test" + tables.ToString() + " VALUES(");
                insert.Append(count.ToString() + ",");
                insert.Append(sqlServer.rows);
                insert.Append(");");
                sqlServer.AddRow(insert.ToString());
            }
            if (oracleServer != null)
            {
                StringBuilder insert = new StringBuilder("Insert into Test" + tables.ToString() + " VALUES(");
                insert.Append(count.ToString() + ",");
                insert.Append(oracleServer.rows);
                insert.Append(");");
                oracleServer.AddRow(insert.ToString());
            }
            if (postgreSQLServer != null)
            {
                StringBuilder insert = new StringBuilder("Insert into Test" + tables.ToString() + " VALUES(");
                insert.Append(count.ToString() + ",");
                insert.Append(postgreSQLServer.rows);
                insert.Append(");");
                postgreSQLServer.AddRow(insert.ToString());
            }
            if (mySQLServer != null)
            {
                StringBuilder insert = new StringBuilder("Insert into Test" + tables.ToString() + " VALUES(");
                insert.Append(count.ToString() + ",");
                insert.Append(mySQLServer.rows);
                insert.Append(");");
                mySQLServer.AddRow(insert.ToString());
            }
            if (mariaDBServer != null)
            {
                StringBuilder insert = new StringBuilder("Insert into Test" + tables.ToString() + " VALUES(");
                insert.Append(count.ToString() + ",");
                insert.Append(mariaDBServer.rows);
                insert.Append(");");
                mariaDBServer.AddRow(insert.ToString());
            }
        }

        // Method will reset each of the systems in preparation for the next rows
        private static void Reset()
        {
            if (sqlServer != null)
            {
                sqlServer.Reset();
            }
            if (oracleServer != null)
            {
                oracleServer.Reset();
            }
            if (postgreSQLServer != null)
            {
                postgreSQLServer.Reset();
            }
            if (mySQLServer != null)
            {
                mySQLServer.Reset();
            }
            if (mariaDBServer != null)
            {
                mariaDBServer.Reset();
            }
        }

        // Method will remove trailing comma from the column list prior to closing
        private static void TrimEnd(bool clear = true)
        {
            if (CreateSQLScript)
            {
                sqlServer.TrimEnd(clear);
            }
            if (CreateOracleScript == true)
            {
                oracleServer.TrimEnd(clear);
            }
            if (CreatePostgreSQLScript == true)
            {
                postgreSQLServer.TrimEnd(clear);
            }
            if (CreateMySQLScript == true)
            {
                mySQLServer.TrimEnd(clear);
            }
            if (CreateMariaDBScript == true)
            {
                mariaDBServer.TrimEnd(clear);
            }
        }
        #endregion

    }
}
