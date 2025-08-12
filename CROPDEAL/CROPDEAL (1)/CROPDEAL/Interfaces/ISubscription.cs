using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CROPDEAL.Models.DTO;
using CROPDEAL.Models;

namespace CROPDEAL.Interfaces
{
    public interface ISubscription
    {
        Task LogToDatabase(string level, string message, DateTime now);
        Task<IEnumerable<Subscription>> GetAllSubscriptions();
        Task<Subscription> GetSubscriptionById(string subscriptionId);
        Task<bool> AddSubscription(SubscriptionDTO subscription);
        Task<bool> UpdateSubscription(SubscriptionDTO subscription);
        Task<bool> DeleteSubscription(string subscriptionId);
    }
}