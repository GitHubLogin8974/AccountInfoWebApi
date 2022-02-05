using AccountInfoWebApi.DTO;
using AccountInfoWebApi.Interface;
using AccountInfoWebApi.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace AccountInfoWebApi.DAL.DataManager
{
    public class ManageAccountInfo : IManageAccountInfo
    {
        #region Variables 
        private readonly IConnectionManager _connectionManager;
        #endregion

        #region Constructor

        public ManageAccountInfo(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }
        #endregion

        #region SaveAccountInfo
        public InsertAccountInfoResponse SaveAccountInfo(AccountInfoRequest requestedData)
        {
            InsertAccountInfoResponse response = new InsertAccountInfoResponse();
            try
            {
                Dictionary<string, string> parameterList = new Dictionary<string, string>
                {
                    {"Code", requestedData.accountInfo.Code},
                    {"Name", requestedData.accountInfo.Name},
                    {"Address", requestedData.accountInfo.Address},
                    {"Country", requestedData.accountInfo.Country},
                    {"State", requestedData.accountInfo.State},
                    {"City", requestedData.accountInfo.City},
                    {"Area", requestedData.accountInfo.Area},
                    {"MobileNumber", requestedData.accountInfo.MobileNumber},
                    {"OpenningBalance", requestedData.accountInfo.OpenningBalance.ToString()},
                    {"OpenningBalanceType", requestedData.accountInfo.OpenningBalanceType},
                    {"CinNum",requestedData.accountInfo.CinNum},
                    {"CINDate", !string.IsNullOrEmpty(requestedData.accountInfo.CINDate.ToString())?requestedData.accountInfo.CINDate.ToString():null},
                    {"GSTDetailJson", (requestedData.GstInfoList.Count > 0)? JsonConvert.SerializeObject(requestedData.GstInfoList):null},
                    {"TDSDetailJson", (requestedData.TDSInfoList.Count > 0)? JsonConvert.SerializeObject(requestedData.TDSInfoList):null}
                };
                List<SqlParameter> outPutParameters = new List<SqlParameter>() {
                    new SqlParameter("@responseId", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    },
                    new SqlParameter("@IsDuplicate", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    },
                    new SqlParameter("@ErrorLog", SqlDbType.VarChar,250)
                    {
                        Direction = ParameterDirection.Output
                    }
                }; 
                var returnValue = _connectionManager.ManageData("[dbo].[InsertAccountInfo]", parameterList, outPutParameters);
                if (returnValue.ContainsKey("responseId"))
                {
                    response.AccountId = Convert.ToInt32(returnValue["responseId"]);
                }
                if (returnValue.ContainsKey("IsDuplicate"))
                {
                    response.IsDuplicateRecord = Convert.ToBoolean(returnValue["IsDuplicate"]);
                }
                return response;
            }
            catch (Exception ex)
            {
                ErrorException exDetail = new ErrorException
                {
                    ErrorTitle = "Error Occured in DAL.DataManager.ManageAccountInfo SaveAccountInfo method.",
                    ExceptionMessage = ex.Message.ToString(),
                    ExceptionStackTrace = ex.StackTrace.ToString()
                };
                throw ex;
            }

        }
        #endregion

        #region UpdateAccountInfo

        public UpdateAccountInfoResponse UpdateAccountInfo(AccountInfoRequest requestedData)
        {
            UpdateAccountInfoResponse response = new UpdateAccountInfoResponse();
            try
            {
                Dictionary<string, string> parameterList = new Dictionary<string, string>
                {
                    {"AccountId", requestedData.accountInfo.AccountId.ToString()},
                    {"Code", requestedData.accountInfo.Code},
                    {"Name", requestedData.accountInfo.Name},
                    {"Address", requestedData.accountInfo.Address},
                    {"Country", requestedData.accountInfo.Country},
                    {"State", requestedData.accountInfo.State},
                    {"City", requestedData.accountInfo.City},
                    {"Area", requestedData.accountInfo.Area},
                    {"MobileNumber", requestedData.accountInfo.MobileNumber},
                    {"OpenningBalance", requestedData.accountInfo.OpenningBalance.ToString()},
                    {"OpenningBalanceType", requestedData.accountInfo.OpenningBalanceType},
                    {"CinNum",requestedData.accountInfo.CinNum},
                    {"CINDate", !string.IsNullOrEmpty(requestedData.accountInfo.CINDate.ToString())?requestedData.accountInfo.CINDate.ToString():null},
                    {"GSTDetailJson", (requestedData.GstInfoList.Count > 0)? JsonConvert.SerializeObject(requestedData.GstInfoList):null},
                    {"TDSDetailJson", (requestedData.TDSInfoList.Count > 0)? JsonConvert.SerializeObject(requestedData.TDSInfoList):null}
                };

                List<SqlParameter> outPutParameters = new List<SqlParameter>() {
                             new SqlParameter("@IsUpdated", SqlDbType.Bit)
                             {
                                Direction = ParameterDirection.Output
                             }
                };
                var returnValue = _connectionManager.ManageData("[dbo].[UpdateAccountInfo]", parameterList, outPutParameters);
                if (returnValue.ContainsKey("IsUpdated"))
                {
                    response.IsUpdated = Convert.ToBoolean(returnValue["IsUpdated"]);
                }
            }
            catch (Exception ex)
            {
                ErrorException exDetail = new ErrorException
                {
                    ErrorTitle = "Error Occured in DAL.DataManager.ManageAccountInfo UpdateAccountInfo method.",
                    ExceptionMessage = ex.Message.ToString(),
                    ExceptionStackTrace = ex.StackTrace.ToString()
                };
                response.IsUpdated = false;
            }
            return response;
        }
        #endregion

        #region GetAccountInformation
        public List<GetAccountInfo> GetAccountInformation() {
            try
            {
                 
                DataTable dtAccount = _connectionManager.GetData("[dbo].[GetAllAccountInfo]"); 
                if (dtAccount.Rows.Count > 0)
                {
                    return CommonFunction.ConvertToList<GetAccountInfo>(dtAccount);
                }
                return new List<GetAccountInfo>();
            }
            catch (Exception ex)
            {
                ErrorException exDetail = new ErrorException
                {
                    ErrorTitle = "Error Occured in DAL.DataManager.ManageAccountInfo SaveAccountInfo method.",
                    ExceptionMessage = ex.Message.ToString(),
                    ExceptionStackTrace = ex.StackTrace.ToString()
                };
                throw ex;
            }
        }
        #endregion
    }
}
