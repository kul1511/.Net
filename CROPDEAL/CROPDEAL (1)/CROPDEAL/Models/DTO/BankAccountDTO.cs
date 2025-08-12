using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CROPDEAL.Models.DTO
{
    public class BankAccountDTO
    {
        public string? BankId { get; set; }
        public string? UserId { get; set; }
        public long AccountNumber { get; set; }
        public string? BankName { get; set; }
        public string? IFSCCode { get; set; }
    }
}