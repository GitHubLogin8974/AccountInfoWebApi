using System;
using System.ComponentModel.DataAnnotations;

namespace AccountInfoWebApi.DTO.BusinessObjects
{
    public class TDSInfo
    {
        public int TdsInfoId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime ApplicableFromDate { get; set; }

        public DateTime? ApplicableTillDate { get; set; }

        [Required]
        public string TdsPercentage { get; set; }
 
    }
}
