using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace WebReporting
{
    public class ResponseObj
    {
        public int total { get; set; }
        public DataTable data { get; set; }
        public List<ColumnObj> columns  { get; set; }
        public string msg { get; set; }
        public string env_id { get; set; }
        public string repo_id { get; set; }
        
        public ResponseObj()
        {
            total = 0;
            data = new DataTable();
            columns = new List<ColumnObj>();
            msg = string.Empty;
            env_id = string.Empty;
            repo_id = string.Empty;
        }
    }
}