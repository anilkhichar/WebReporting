using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;
using System.Data;

namespace WebReporting
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IReportingDataService" in both code and config file together.
    [ServiceContract]
    public interface IReportingService
    {
        [OperationContract]
        [WebGet(    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    UriTemplate = "getjson?type={type}&env_id={env_id}&repo_id={repo_id}&date_val={date_val}"
                )
        ]
        string GetJSON(string type, string env_id, string repo_id, string date_val);
    }
}
