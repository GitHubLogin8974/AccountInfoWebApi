using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace AccountInfoWebApi.DTO.BusinessObjects
{
    [DataContract]
    public class AccountInfo
    {
        public int AccountId { get; set; }

        [DataMember]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Please provide valid code.")]
        [Required(ErrorMessage = "Please provide the code.")]
        public string Code { get; set; }
         
        [MaxLength(50)]
        [DataType(DataType.Text)]
        [DataMember(IsRequired = true)]
        [Required(ErrorMessage = "Please provide your name.")]
        public string Name { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public string State { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string Area { get; set; }

        [DataMember(IsRequired = true)]
        [Required(ErrorMessage = "Please provide your mobile number")]
        public string MobileNumber { get; set; }

        [DataMember]
        public float OpenningBalance { get; set; }

        [DataMember]
        public string OpenningBalanceType { get; set; }

        [DataMember]
        public string CinNum { get; set; }

        [DataType(DataType.Date)]
        [DataMember]
        public DateTime? CINDate { get; set; }
    }
}
