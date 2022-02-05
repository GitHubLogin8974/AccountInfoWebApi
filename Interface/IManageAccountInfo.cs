using AccountInfoWebApi.DTO;
using System.Collections.Generic;

namespace AccountInfoWebApi.Interface
{
    public interface IManageAccountInfo
    {
        InsertAccountInfoResponse SaveAccountInfo(AccountInfoRequest requestedData);

        UpdateAccountInfoResponse UpdateAccountInfo(AccountInfoRequest requestedData);

        List<GetAccountInfo> GetAccountInformation();
    }
}
