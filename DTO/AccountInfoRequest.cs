using System.Collections.Generic;
using AccountInfoWebApi.DTO.BusinessObjects;

namespace AccountInfoWebApi.DTO
{
    public class AccountInfoRequest
    {
        public AccountInfo accountInfo { get; set; }

        public List<GSTInfo> GstInfoList { get; set; }

        public List<TDSInfo> TDSInfoList { get; set; }
    }
}
