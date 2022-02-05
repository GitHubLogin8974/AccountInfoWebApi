using AccountInfoWebApi.DTO;

namespace AccountInfoWebApi.Utility
{
    public class ApiResponseManager
    {
        public ResponseManager<T> GetResponse<T>(ResponseManager<T> ResponseData)
        {
            return new ResponseManager<T> { Action = ResponseData.Action, ResponseMessage = ResponseData.ResponseMessage, Status = ResponseData.Status, detail = ResponseData.detail };
        }
    }
}
