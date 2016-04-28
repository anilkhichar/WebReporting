using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebReporting
{
    public class ColumnObj
    {
        public string field { get; set; }
        public string title { get; set; }
        public bool show { get; set; }
        public Dictionary<string, string> filter { get; set; }
        public string sortable { get; set; }
        // { field: "age", title: "Age", show: true }
        public ColumnObj()
        {
            field = string.Empty;
            title = string.Empty;
            show = true;
            filter = new Dictionary<string, string>();
            sortable = string.Empty;
        }
    }
}