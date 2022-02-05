using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace AccountInfoWebApi.Interface
{
    public interface IConnectionManager
    {
        DataTable GetData(string queryName, Dictionary<string, string> parametersList = null);

        Dictionary<string, object> ManageData(string queryName, Dictionary<string, string> ParametersList, List<SqlParameter> outPutParameter = null);
    }
}
