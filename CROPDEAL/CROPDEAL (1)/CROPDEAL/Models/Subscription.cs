using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CROPDEAL.Models
{
    public class Subscription
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string? SubscriptionId { get; set; }
        public string? UserId { get; set; }
        public string? CropType { get; set; }
        public DateTime SubscribedOn { get; set; }
        [Required]
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}