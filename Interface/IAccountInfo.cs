using AccountInfoWebApi.DTO;
using System.Collections.Generic;

namespace AccountInfoWebApi.Interface
{
    public interface IAccountInfo
    {
        ResponseManager<InsertAccountInfoResponse> InsertAccountInformation(AccountInfoRequest RequestedData);
        ResponseManager<UpdateAccountInfoResponse> UpdateAccountInformation(AccountInfoRequest RequestedData);

        ResponseManager<List<GetAccountInfo>> GetAllAccountInfo();
    }
}
