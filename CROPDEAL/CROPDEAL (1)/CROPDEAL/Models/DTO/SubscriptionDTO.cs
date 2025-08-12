using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CROPDEAL.Models.DTO
{
    public class SubscriptionDTO
    {
        public string? SubscriptionId { get; set; }
        public string? UserId { get; set; }
        public string? CropType { get; set; }
        public DateTime SubscribedOn { get; set; }
    }
}