using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountInfoWebApi.DTO
{
    public class ResponseManager<T>
    {
        public string Status { get; set; } 
        public string Action { get; set; }
        public string ResponseMessage { get; set; }
        public T detail { get; set; }
        public ErrorException errorDetail { get; set; }
    }
}
