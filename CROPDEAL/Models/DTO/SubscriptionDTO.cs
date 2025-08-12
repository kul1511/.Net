using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CROPDEAL.Models.DTO
{
    public class SubscriptionDTO
    {
        [JsonIgnore]
        public int? SubscriptionId { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
        public string? CropType { get; set; }
        public DateTime SubscribedOn { get; set; }
    }
}