using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RandomDatabaseGenerator
{
    abstract class rdbms
    {
        #region Properties
        public StringBuilder table = new StringBuilder();
        public StringBuilder rows = new StringBuilder();
        private string fileName = "";
        protected List<string> cols = new List<string>();
        protected List<string> vals = new List<string>();
        public FileStream scriptFileStream = null;
        private string scriptFolderPath;

        public string ScriptFolderPath { get => scriptFolderPath; set => scriptFolderPath = value; }
        public string FileName { get => fileName; set => fileName = value; }
        #endregion Properties

        public rdbms(string scriptFolderPath,string fileName)
        {
            ScriptFolderPath = scriptFolderPath;
            FileName = fileName;
            scriptFileStream = File.Create(ScriptFolderPath + FileName);
        }
        public virtual string GetColumnName(int index) 
        {
            string ret = "";
            if (cols != null)
            {
                ret = cols[index];
            }
            return ret;
        }
        public virtual string GetColumnValue(int index)
        {
            string ret = "";
            if (vals != null)
            {
                ret = vals[index];
            }
            return ret;
        }
        public virtual void InitializeDB(string databaseName)
        {

        }
        public void AddToScript(string data, bool addCR = false)
        {
            if (scriptFileStream != null)
            {
                data += (addCR) ? "\r\n" : "";
                Byte[] info = new UTF8Encoding(false).GetBytes(data);
                scriptFileStream.Write(info, 0, info.Length);
            }
        }        
        public void AddColumn(string columnNumber, int index)
        {
            table.Append(string.Format("Col{0} {1}", columnNumber, cols[index]+","));
            string comma = (rows.Length > 0) ? "," : ""; 
            rows.Append(comma + vals[index]);
        }
        public void AddRow(string row)
        {
            AddToScript(row,true);
        }
        public void TrimEnd(bool clear = true)
        {
            if (table[table.Length - 1] == ',')
            {
                table.Remove(table.Length - 1, 1);
            }
            table.Append(");");
            AddToScript(table.ToString(),true);
            if (clear)
            {
                table.Clear();
            }
        }
        public void FlushAndClose()
        {
            if (scriptFileStream != null)
            {
                scriptFileStream.Flush();
                scriptFileStream.Close();
            }
        }
        public void Reset()
        {
            table.Clear();
            rows.Clear();
        }
    }
}
