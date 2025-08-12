using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CROPDEAL.Models;
using CROPDEAL.Data;
using CROPDEAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using CROPDEAL.Models.DTO;
using AutoMapper;

namespace CROPDEAL.Repository
{
    public class SubscriptionRepository : ISubscription
    {
        private readonly IMapper mapper;
        private readonly CropDealDbContext _context;
        public SubscriptionRepository(CropDealDbContext context, IMapper _mapper)
        {
            _context = context;
            mapper = _mapper;
        }
        public async Task LogToDatabase(string level, string message, DateTime now)
        {
            var log = new Log
            {
                LogLevel = level,
                Message = message,
                Timestamp = now
            };
            await _context.Logs.AddAsync(log);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Subscription>> GetAllSubscriptions()
        {
            await LogToDatabase("Info", "Getting All Subscriptions Details...", DateTime.Now);
            var res = await _context.Subscriptions.ToListAsync();
            if (res != null)
            {
                await LogToDatabase("Success", "Successfully Retrieved Data", DateTime.Now);
                return res;
            }
            await LogToDatabase("Info", "Table does not contain any Data", DateTime.Now);
            return res;
        }
        public async Task<Subscription> GetSubscriptionById(string subscriptionId)
        {
            var res = await _context.Subscriptions.FirstOrDefaultAsync(u => u.SubscriptionId == subscriptionId);
            if (res != null)
            {
                await LogToDatabase("Success", $"Successfully Retrieved Subscription: {res}", DateTime.Now);
                return res;
            }
            await LogToDatabase("Info", $"Table does not contain Subscription with Id: {subscriptionId}", DateTime.Now);
            return res;
        }
        public async Task<bool> AddSubscription(SubscriptionDTO newSub)
        {
            var sub = mapper.Map<Subscription>(newSub);

            if (!await _context.Crops.AnyAsync(c => c.CropType == newSub.CropType))
            {
                await LogToDatabase("Failed", $"The Crop you are trying to add does not exists, Crop Type: {newSub.CropType}", DateTime.Now);
                return false;
            }

            var userRole = await _context.Users.FirstOrDefaultAsync(u => u.UserId == newSub.UserId);

            if (userRole.Role == UserRole.Farmer)
            {
                await LogToDatabase("Failed", "Trying to Add Crop to a Farmer.", DateTime.Now);
                return false;
            }
            if (userRole.Role == UserRole.Admin)
            {
                await LogToDatabase("Failed", "Trying to Add Crop to a Admin.", DateTime.Now);
                return false;
            }

            var checkSub = await _context.Subscriptions.FirstOrDefaultAsync(u => u.SubscriptionId == newSub.SubscriptionId);
            if (checkSub == null)
            {
                await _context.Subscriptions.AddAsync(sub);
                await _context.SaveChangesAsync();
                await LogToDatabase("Success", $"Successfully Added Subscription with Id: {newSub.SubscriptionId}", DateTime.Now);
                return true;
            }
            await LogToDatabase("Info", $"Table already contains Subscription with Id: {newSub.SubscriptionId}", DateTime.Now);
            return false;
        }

        public async Task<bool> UpdateSubscription(SubscriptionDTO updateSub)
        {
            var sub = mapper.Map<Subscription>(updateSub);
            var checkSub = await _context.Subscriptions.FirstOrDefaultAsync(u => u.SubscriptionId == updateSub.SubscriptionId);
            if (checkSub != null)
            {
                checkSub.CropType = sub.CropType;
                checkSub.SubscribedOn = sub.SubscribedOn;
                await _context.SaveChangesAsync();
                await LogToDatabase("Success", $"Successfully Updated Subscription with Id: {updateSub.SubscriptionId}", DateTime.Now);
                return true;
            }
            await LogToDatabase("Info", $"Table does not contain Subscription with Id: {updateSub.SubscriptionId}", DateTime.Now);
            return false;
        }
        public async Task<bool> DeleteSubscription(string subscriptionId)
        {
            var checkSub = await _context.Subscriptions.FirstOrDefaultAsync(u => u.SubscriptionId == subscriptionId);
            if (checkSub != null)
            {
                _context.Remove(checkSub);
                await _context.SaveChangesAsync();
                await LogToDatabase("Success", $"Successfully Deleted Subscription with Id: {subscriptionId}", DateTime.Now);
                return true;
            }
            await LogToDatabase("Info", $"Table does not contain Subscription with Id: {subscriptionId}", DateTime.Now);
            return false;
        }
    }
}