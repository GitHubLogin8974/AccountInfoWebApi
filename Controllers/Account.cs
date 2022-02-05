using AccountInfoWebApi.DTO;
using AccountInfoWebApi.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountInfoWebApi.Controllers
{
    [ApiController]
    [EnableCors("CORS-Policy")]
    [Route("api/[controller]/")]
    public class Account : ControllerBase
    {
        #region Variable
        private readonly IAccountInfo _AccountInfo;
        #endregion

        #region Constructor
        public Account(IAccountInfo AccountInfo)
        {
            _AccountInfo = AccountInfo;
        }
        #endregion

        #region Insert Account Detail
        [HttpPost]
        [Route("Insert")]
        public ResponseManager<InsertAccountInfoResponse> Insert(AccountInfoRequest accountInfoRequest)
        {
            return _AccountInfo.InsertAccountInformation(accountInfoRequest);
        }
        #endregion

        #region Update Account Detail
        [HttpPut]
        public ResponseManager<UpdateAccountInfoResponse> Update(AccountInfoRequest accountInfoRequest)
        {
            return _AccountInfo.UpdateAccountInformation(accountInfoRequest);
        }
        #endregion

        #region Get Account Detail
        [HttpGet]
        [Route("GetAll")]
        public async Task<ResponseManager<List<GetAccountInfo>>> GetAll()
        {
            ResponseManager<List<GetAccountInfo>> response = null;
            await Task.Run(() =>
            {
                response = _AccountInfo.GetAllAccountInfo();

            });
            return response;
        }
        #endregion
    }
}
