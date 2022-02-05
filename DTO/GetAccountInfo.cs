using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountInfoWebApi.DTO
{//  
    public class GetAccountInfo
    { 
            public int Id { get; set; }
            public string Name { get; set; }
            public string MobileNumber { get; set; }
            public string Address { get; set; }
            
            public string GSTNUMBER { get; set; }
            public string TdsPercentage { get; set; }
        
    }
}
