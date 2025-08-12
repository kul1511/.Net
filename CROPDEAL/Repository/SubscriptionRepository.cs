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
        ILogger<SubscriptionRepository> log;
        private readonly IMapper mapper;
        private readonly CropDealDbContext _context;
        public SubscriptionRepository(CropDealDbContext context, IMapper _mapper, ILogger<SubscriptionRepository> _log)
        {
            _context = context;
            mapper = _mapper;
            log = _log;
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
            log.LogInformation("Getting All Subscriptions Details...", DateTime.Now);
            var res = await _context.Subscriptions.ToListAsync();
            if (res != null)
            {
                log.LogInformation("Successfully Retrieved Data", DateTime.Now);
                return res;
            }
            log.LogInformation("Table does not contain any Data", DateTime.Now);
            return res;
        }
        public async Task<Subscription> GetSubscriptionById(int subscriptionId)
        {
            var res = await _context.Subscriptions.FirstOrDefaultAsync(u => u.SubscriptionId == subscriptionId);
            if (res != null)
            {
                log.LogInformation($"Successfully Retrieved Subscription: {res}", DateTime.Now);
                return res;
            }
            log.LogInformation($"Table does not contain Subscription with Id: {subscriptionId}", DateTime.Now);
            return res;
        }
        public async Task<bool> AddSubscription(SubscriptionDTO newSub)
        {
            var sub = mapper.Map<Subscription>(newSub);

            if (!await _context.Crops.AnyAsync(c => c.CropType == newSub.CropType))
            {
                log.LogError($"The Crop you are trying to add does not exists, Crop Type: {newSub.CropType}", DateTime.Now);
                return false;
            }

            var userRole = await _context.Users.FirstOrDefaultAsync(u => u.UserId == newSub.UserId);

            if (userRole.Role == UserRole.Farmer)
            {
                log.LogError("Trying to Add Crop to a Farmer.", DateTime.Now);
                return false;
            }
            if (userRole.Role == UserRole.Admin)
            {
                log.LogError("Trying to Add Crop to a Admin.", DateTime.Now);
                return false;
            }

            var checkSub = await _context.Subscriptions.FirstOrDefaultAsync(u => u.SubscriptionId == newSub.SubscriptionId);
            if (checkSub == null)
            {
                await _context.Subscriptions.AddAsync(sub);
                await _context.SaveChangesAsync();
                log.LogInformation($"Successfully Added Subscription with Id: {newSub.SubscriptionId}", DateTime.Now);
                return true;
            }
            log.LogInformation($"Table already contains Subscription with Id: {newSub.SubscriptionId}", DateTime.Now);
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
                log.LogInformation($"Successfully Updated Subscription with Id: {updateSub.SubscriptionId}", DateTime.Now);
                return true;
            }
            log.LogInformation($"Table does not contain Subscription with Id: {updateSub.SubscriptionId}", DateTime.Now);
            return false;
        }
        public async Task<bool> DeleteSubscription(int subscriptionId)
        {
            var checkSub = await _context.Subscriptions.FirstOrDefaultAsync(u => u.SubscriptionId == subscriptionId);
            if (checkSub != null)
            {
                _context.Remove(checkSub);
                await _context.SaveChangesAsync();
                log.LogInformation($"Successfully Deleted Subscription with Id: {subscriptionId}", DateTime.Now);
                return true;
            }
            log.LogInformation($"Table does not contain Subscription with Id: {subscriptionId}", DateTime.Now);
            return false;
        }
    }
}