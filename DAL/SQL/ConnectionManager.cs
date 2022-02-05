using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using AccountInfoWebApi.Interface;

namespace AccountInfoWebApi.DAL.SQL
{
    public class ConnectionManager: IConnectionManager
    {
        #region Variables
        public enum SqlCommandType : byte { Query = 1, StoreProcedure };
        private readonly SqlCommand _sqlCommand;
        private readonly SqlConnection _sqlConnection;
        private DTO.ErrorException ErrorDetail = null;
        #endregion

        #region Constructor

        public ConnectionManager()
        {

            _sqlConnection = new SqlConnection(Startup.StaticConfig.GetSection("ConnectionStrings").GetSection("OmTechErpConnection").Value.ToString());
            _sqlCommand = new SqlCommand()
            {
                Connection = _sqlConnection
            };
        }
        #endregion

        #region SQL Methods
        private void GetSqlConnection()
        {
            if (_sqlConnection.State == ConnectionState.Closed)
            {
                _sqlConnection.Open();
            }
        }

        private void CloseSqlConnection()
        {
            if (_sqlConnection != null && _sqlConnection.State == ConnectionState.Open)
            {
                _sqlConnection.Close();
            }
        }

        #endregion

        #region GetData
        public DataTable GetData(string queryName, Dictionary<string, string> parametersList = null)
        {
            SqlDataAdapter sqlDataAdapter;
            DataSet dsReturn;
            try
            {
                GetSqlConnection();
                _sqlCommand.CommandText = queryName;
                if (parametersList != null)
                {
                    foreach (var Parameter in parametersList.Keys)
                    {
                        if (string.IsNullOrEmpty(parametersList[Parameter]))
                        {
                            _sqlCommand.Parameters.AddWithValue(Parameter.ToString(), DBNull.Value);
                        }
                        else
                        {
                            _sqlCommand.Parameters.AddWithValue(Parameter.ToString(), parametersList[Parameter]);
                        }
                    }
                }
                sqlDataAdapter = new SqlDataAdapter();
                _sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlDataAdapter.SelectCommand = _sqlCommand;

                dsReturn = new DataSet();
                sqlDataAdapter.Fill(dsReturn);
                return dsReturn.Tables[0];
            }

            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                CloseSqlConnection();
            }
        }
        #endregion

        #region CRUD OPERATION METHOD
        public Dictionary<string, object> ManageData(string queryName, Dictionary<string, string> ParametersList, List<SqlParameter> outPutParameter = null)
        {
            SqlTransaction sqlTransaction = null;
            try
            {
                GetSqlConnection();
                sqlTransaction = _sqlConnection.BeginTransaction(IsolationLevel.ReadCommitted);
                _sqlCommand.Transaction = sqlTransaction;
                _sqlCommand.CommandText = queryName;
                _sqlCommand.Parameters.Clear();
                if (ParametersList != null)
                {
                    foreach (var parameter in ParametersList.Keys)
                    {
                        if (string.IsNullOrEmpty(ParametersList[parameter]))
                        {
                            _sqlCommand.Parameters.AddWithValue(parameter.ToString(), DBNull.Value);
                        }
                        else
                        {
                            _sqlCommand.Parameters.AddWithValue(parameter.ToString(), ParametersList[parameter]);
                        }
                    }
                }
                if (outPutParameter != null)
                {
                    foreach (var parameter in outPutParameter)
                    {
                        _sqlCommand.Parameters.Add(parameter);
                    }
                }
                _sqlCommand.CommandType = CommandType.StoredProcedure;
                _sqlCommand.ExecuteNonQuery();
                sqlTransaction.Commit();
                return SetkeyValuePairs(outPutParameter);
            }
            catch (SqlException ex)
            {
                ErrorDetail.ErrorTitle = "SQL ERROR CODE :" + ex.ErrorCode.ToString();
                ErrorDetail.ExceptionMessage = ex.Message.ToString();
                ErrorDetail.ExceptionStackTrace = ex.StackTrace.ToString();
                sqlTransaction.Rollback();
                return SetkeyValuePairs(null, ErrorDetail);
            }
            catch (Exception ex)
            {
                ErrorDetail.ErrorTitle = "Error Occured in DAL.ConnectionManager ManageData Method.";
                ErrorDetail.ExceptionMessage = ex.Message.ToString();
                ErrorDetail.ExceptionStackTrace = ex.StackTrace.ToString();
                sqlTransaction.Rollback();
                return SetkeyValuePairs(null, ErrorDetail);
            }
            finally
            {
                CloseSqlConnection();
            }
        }
        #endregion

        #region SetkeyValuePairs
        private Dictionary<string, object> SetkeyValuePairs(List<SqlParameter> SqlOutPutParameter = null, DTO.ErrorException ErrorDetail = null)
        {
            Dictionary<string, object> keyValues = new Dictionary<string, object>();

            if (SqlOutPutParameter != null && ErrorDetail == null)
            {
                keyValues.Add("isSuccess", true);
                foreach (SqlParameter output in SqlOutPutParameter)
                {
                    keyValues.Add(output.ParameterName.Replace("@", ""), output.Value);
                }
            }
            else if (ErrorDetail != null && SqlOutPutParameter == null)
            {
                keyValues.Add("isSuccess", false);
                keyValues.Add("ErrorDetail", ErrorDetail);
            }
            else
            {
                keyValues.Add("isSuccess", true);
            }
            return keyValues;
        }
        #endregion
    }
}
