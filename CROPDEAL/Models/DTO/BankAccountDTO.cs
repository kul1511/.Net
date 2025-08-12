using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CROPDEAL.Models.DTO
{
    public class BankAccountDTO
    {
        [JsonIgnore]
        public int? BankId { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
        [RegularExpression(@"^\d{12}$", ErrorMessage = "Account number must be exactly 12 digits.")]
        public long AccountNumber { get; set; }
        public string? BankName { get; set; }
        public string? IFSCCode { get; set; }
    }
}