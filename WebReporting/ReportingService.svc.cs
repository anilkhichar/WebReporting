using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace WebReporting
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ReportingDataService" in code, svc and config file together.
    
    public class ReportingService : IReportingService
    {
        public string GetJSON(string type, string env_id, string repo_id, string date_val)
        {
            if (type == "setup") return GetSetupJSON(); // if setup json is required, no need to go further

            string connection_string = "", query="";
            var rs = new ResponseObj();
            rs.env_id = env_id;
            rs.repo_id = repo_id;
            try
            {
                JObject json = JObject.Parse(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"\query-setup.json"));
                foreach (JObject obj in json["environments"]) if (obj["id"].ToString() == env_id) connection_string = obj["connection_string"].ToString();
                foreach (JObject obj in json["repositories"]) if (obj["id"].ToString() == repo_id)
                {
                    query = obj["query"].ToString();
                    if (query.ToLower().Contains("@date")) query = query.Replace("@date", date_val);
                    // example: select top 2000 * FROM Table_Name where ( convert(date, Last_Modified_Date) = '2016/04/12' or '1'='')
                }
                rs = GetDataByQuery(connection_string, query, rs);
            }
            catch (Exception ex) // any unexpected error
            {
                rs.msg = ex.StackTrace.Replace("C:\\Users\\ak35035\\Documents\\Visual Studio 2010\\Projects\\", ""); // hack to remove dev machine name
            }
            return JsonConvert.SerializeObject(rs);
        }

        public ResponseObj GetDataByQuery(string connection_string, string query, ResponseObj rs)
        {
            if (query.ToLower().Contains("delete") || query.ToLower().Contains("drop") || query.ToLower().Contains("sys."))
            {
                rs.msg = "Hmmm...Are you trying to do sql injection??..No Way";
                return rs;
            }
            //query = "select top 2 * FROM  PRTAPP.PRT_OBJECTs";
            //connection_string = "Server=AD1HFDSTA901;Database=Applications;User Id=BATCH_ADMIN;Password=BARun12!;";
            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                try { connection.Open(); }
                catch 
                { 
                    rs.msg = "Please correct the connection string and try again.";
                    return rs;
                }
                try
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    rs.data.Load(reader);
                    rs.msg = "success";
                }
                catch 
                { 
                    rs.msg = "Please correct the query and try again.";
                    return rs;
                }
            }
            rs.total = rs.data.Rows.Count;
            foreach (DataColumn dcol in rs.data.Columns)
            {
                var column = new ColumnObj();
                column.field = dcol.ColumnName;
                column.title = dcol.ColumnName;
                column.show = true;
                column.filter[dcol.ColumnName] = dcol.DataType.Name.ToLower().Contains("int") ? "number" : "text";
                column.sortable = dcol.ColumnName;
                rs.columns.Add(column);
            }            
            return rs;
        }

        public string GetSetupJSON()
        {
            JObject json = JObject.Parse(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"\query-setup.json"));
            if (json.Property("environments") != null)
            {
                foreach (JObject obj in json["environments"])
                {
                    if (obj.Property("connection_string") != null) obj["connection_string"] = ""; // for security purpose balnk it's value before sending back to clients
                }
            }
            if (json.Property("repositories") != null)
            {
                foreach (JObject obj in json["repositories"])
                {
                    if (obj.Property("query") != null) obj["query"] = ""; // for security purpose balnk it's value before sending back to clients
                }
            }
            return JsonConvert.SerializeObject(json);
        }
    }
}
