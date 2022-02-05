using System;
using System.Linq;
using AccountInfoWebApi.DTO;
using AccountInfoWebApi.Utility;
using System.Collections.Generic;
using AccountInfoWebApi.Interface;

namespace AccountInfoWebApi.BLL
{
    public class AccountInfo : IAccountInfo
    {
        #region Variables 
        private string action = string.Empty;
        private readonly string success = ApiConstants.ApiSuccess;
        private readonly string Failed = ApiConstants.ApiFailed;
        private readonly ApiResponseManager _ResponseManager;
        private readonly IManageAccountInfo _AccountInfo;
        private ResponseManager<InsertAccountInfoResponse> ApiResponse = null;
        #endregion

        #region Constructor
        public AccountInfo(IManageAccountInfo AccountInfo)
        {
            _AccountInfo = AccountInfo;
            _ResponseManager = new ApiResponseManager();
            ApiResponse = new ResponseManager<InsertAccountInfoResponse>();
        }

        #endregion

        #region InsertAccountInformation
        public ResponseManager<InsertAccountInfoResponse> InsertAccountInformation(AccountInfoRequest RequestedData)
        {
            InsertAccountInfoResponse response = null;
            action = ApiConstants.ApiInsert;
            try
            {
                if (RequestedData == null)
                {
                    return new ResponseManager<InsertAccountInfoResponse>
                    {
                        Status = Failed,
                        Action = action,
                        ResponseMessage = "The requested data can't be null or empty",
                        detail = new InsertAccountInfoResponse
                        {
                            AccountId = -1,
                            IsDuplicateRecord = false
                        }
                    };
                }

                var isValidAccountInfo = CommonFunction.ValidData(RequestedData.accountInfo);
                var isValidGSTInfo = (RequestedData.GstInfoList.Count > 0) ? CommonFunction.ValidData(RequestedData.GstInfoList) : null;
                var isValidTDSInfo = (RequestedData.TDSInfoList.Count > 0) ? CommonFunction.ValidData(RequestedData.TDSInfoList) : null;
                if (isValidAccountInfo == null && isValidGSTInfo == null && isValidTDSInfo == null)
                {
                    response = _AccountInfo.SaveAccountInfo(RequestedData);
                    if (response.AccountId >= 0)
                    {
                        return new ResponseManager<InsertAccountInfoResponse>
                        {
                            Status = success,
                            Action = action,
                            ResponseMessage = (!response.IsDuplicateRecord) ? "The requested accountInfo saved successfully." : "The provided detail already exists.",
                            detail = response
                        };
                    }
                    else
                    {
                        return new ResponseManager<InsertAccountInfoResponse>
                        {
                            Status = Failed,
                            Action = action,
                            ResponseMessage = "Failed to insert the requested data.",
                            detail = response
                        };
                    }
                }
                else
                {
                    List<ErrorException> ValidationList = new List<ErrorException>();
                    if (isValidAccountInfo != null)
                    {
                        ValidationList.Add(isValidAccountInfo);
                    }
                    if (isValidGSTInfo != null)
                    {
                        ValidationList.Add(isValidGSTInfo);
                    }
                    if (isValidTDSInfo != null)
                    {
                        ValidationList.Add(isValidTDSInfo);
                    }
                    return new ResponseManager<InsertAccountInfoResponse>
                    {
                        Status = Failed,
                        Action = action,
                        ResponseMessage = "There are certain invalid data within the request object." + ValidationList.SingleOrDefault().ErrorTitle,
                        detail = null
                    };
                }

            }
            catch (Exception ex)
            {
                var errorDetail = new ErrorException
                {
                    ErrorTitle = "Error Occured in BLL.BusinessLogic.InsertStackHolderFirmDetail InsertStackHolderFirmData method.",
                    ExceptionMessage = ex.Message.ToString(),
                    ExceptionStackTrace = ex.StackTrace.ToString()
                };
                return new ResponseManager<InsertAccountInfoResponse>
                {
                    Status = Failed,
                    Action = action,
                    ResponseMessage = "Something went wrong. Failed to save the requested data.",
                    detail = null,
                    errorDetail = new ErrorException
                    {
                        ExceptionMessage = ex.Message.ToString(),
                        ExceptionStackTrace = ex.StackTrace.ToString()
                    }
                };
            }
        }
        #endregion

        #region UpdateAccountInformation
        public ResponseManager<UpdateAccountInfoResponse> UpdateAccountInformation(AccountInfoRequest RequestedData)
        {
            UpdateAccountInfoResponse response = null;
            try
            {
                action = ApiConstants.ApiUpdate;
                if (RequestedData == null)
                {
                    return new ResponseManager<UpdateAccountInfoResponse>
                    {
                        Status = Failed,
                        Action = action,
                        ResponseMessage = "The requested data can't be null or empty",
                        detail = new UpdateAccountInfoResponse
                        {
                            IsUpdated = false
                        }
                    };
                }
                var isValidAccountInfo = CommonFunction.ValidData(RequestedData.accountInfo);
                var isValidGSTInfo = (RequestedData.GstInfoList.Count > 0) ? CommonFunction.ValidData(RequestedData.GstInfoList) : null;
                var isValidTDSInfo = (RequestedData.TDSInfoList.Count > 0) ? CommonFunction.ValidData(RequestedData.TDSInfoList) : null;
                if (isValidAccountInfo == null && isValidGSTInfo == null && isValidTDSInfo == null)
                {
                    response = _AccountInfo.UpdateAccountInfo(RequestedData);
                    if (response.IsUpdated)
                    {
                        return new ResponseManager<UpdateAccountInfoResponse>
                        {
                            Status = success,
                            Action = action,
                            ResponseMessage = "The requested accountInfo updated successfully.",
                            detail = response
                        };
                    }
                    else
                    {
                        return new ResponseManager<UpdateAccountInfoResponse>
                        {
                            Status = Failed,
                            Action = action,
                            ResponseMessage = "Failed to update the requested data.",
                            detail = response
                        };
                    }
                }
                else
                {
                    List<ErrorException> ValidationList = new List<ErrorException>();
                    if (isValidAccountInfo != null)
                    {
                        ValidationList.Add(isValidAccountInfo);
                    }
                    if (isValidGSTInfo != null)
                    {
                        ValidationList.Add(isValidGSTInfo);
                    }
                    if (isValidTDSInfo != null)
                    {
                        ValidationList.Add(isValidTDSInfo);
                    }
                    return new ResponseManager<UpdateAccountInfoResponse>
                    {
                        Status = Failed,
                        Action = action,
                        ResponseMessage = "There are certain invalid data within the request object." + ValidationList.SingleOrDefault().ErrorTitle,
                        detail = null
                    };
                }

            }
            catch (Exception ex)
            { 
                return new ResponseManager<UpdateAccountInfoResponse>
                {
                    Status = Failed,
                    Action = action,
                    ResponseMessage = "Something went wrong. Failed to update the requested data.",
                    detail = null,
                    errorDetail = new ErrorException
                    { 
                        ExceptionMessage = ex.Message.ToString(),
                        ExceptionStackTrace = ex.StackTrace.ToString()
                    }
                };

            }
        }
        #endregion

        #region GetAllAccountInfo
        public ResponseManager<List<GetAccountInfo>> GetAllAccountInfo() {
            action = ApiConstants.ApiGet;
            try
            {
                List<GetAccountInfo> responseList = _AccountInfo.GetAccountInformation();
                if (responseList.Count > 0)
                {
                    return new ResponseManager<List<GetAccountInfo>>
                    {
                        Status = success,
                        Action = action,
                        detail = responseList,
                        ResponseMessage = "The account information retreived successfully."
                    };
                }
                return new ResponseManager<List<GetAccountInfo>>
                {
                    Status = success,
                    Action = action,
                    detail = responseList,
                    ResponseMessage = "No record found."
                };
            }
            catch (Exception ex)
            {
                var errorDetail = new ErrorException
                {
                    ErrorTitle = "Error Occured in BLL.BusinessLogic.InsertStackHolderFirmDetail InsertStackHolderFirmData method.",
                    ExceptionMessage = ex.Message.ToString(),
                    ExceptionStackTrace = ex.StackTrace.ToString()
                };
                return new ResponseManager<List<GetAccountInfo>>
                {
                    Status = Failed,
                    Action = action,
                    ResponseMessage = "Something went wrong. Failed to retreive the requested data.",
                    detail = null,
                    errorDetail = new ErrorException
                    {
                        ExceptionMessage = ex.Message.ToString(),
                        ExceptionStackTrace = ex.StackTrace.ToString()
                    }
                };
            }
        }
        #endregion
    }
}
