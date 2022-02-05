using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace AccountInfoWebApi.DTO.BusinessObjects
{
    public class GSTInfo
    {
        public int GSTInfoId { get; set; }

        [DataMember(IsRequired = true)]
        [DataType(DataType.Date)]
        public DateTime ApplicableFromDate { get; set; }

        public DateTime? ApplicableTillDate { get; set; }

        [DataMember(IsRequired = true)]
        [Required(ErrorMessage = "Please provide your GST number")]
        public string GSTNumber { get; set; }

        [DataMember(IsRequired = true)]
        [Required(ErrorMessage = "Please provide your TaxPay Type.")]
        public string TaxPayType { get; set; }
    }
}
