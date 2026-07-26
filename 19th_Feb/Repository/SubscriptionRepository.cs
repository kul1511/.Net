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
using System.Security.Claims;
using log4net;

namespace CROPDEAL.Repository
{
    public class SubscriptionRepository : ISubscription
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(SubscriptionRepository));
        private readonly IMapper mapper;
        private readonly CropDealDbContext _context;
        public SubscriptionRepository(CropDealDbContext context, IMapper _mapper)
        {
            _context = context;
            mapper = _mapper;
        }
        public async Task<IEnumerable<Subscription>> GetAllSubscriptions()
        {
            _logger.Info("Getting All Subscriptions Details...");
            var res = await _context.Subscriptions.ToListAsync();
            if (res != null)
            {
                _logger.Info("Successfully Retrieved Data");
                return res;
            }
            _logger.Info("Table does not contain any Data");
            return res!;
        }

        public async Task<Subscription> GetSubscriptionById(string subscriptionId)
        {
            var res = await _context.Subscriptions.FirstOrDefaultAsync(u => u.SubscriptionId == subscriptionId);
            if (res != null)
            {
                _logger.Info($"Successfully Retrieved Subscription: {subscriptionId}");
                return res;
            }
            _logger.Warn($"Table does not contain Subscription with Id: {subscriptionId}");
            return res!;
        }

        public async Task<bool> AddSubscription(SubscriptionDTO newSub, string userEmail)
        {
            var sub = mapper.Map<Subscription>(newSub);
            sub.SubscribedOn = DateTime.Now;
            var userId = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (userId == null || string.IsNullOrEmpty(userId.Email))
            {
                _logger.Warn($"User Email could not be found for Subscribing: {userEmail}");
                return false;
            }
            _logger.Info($"User Id for Subscribing: {userId!.UserId}");
            sub.UserId = userId.UserId;
            if (!await _context.Crops.AnyAsync(c => c.CropId == newSub.CropId))
            {
                _logger.Warn($"The Crop you are trying to add does not exists, Crop Type: {newSub.CropId}");
                return false;
            }
            var checkSub = await _context.Subscriptions.FirstOrDefaultAsync(u => u.SubscriptionId == newSub.SubscriptionId);
            if (checkSub == null)
            {
                await _context.Subscriptions.AddAsync(sub);
                await _context.SaveChangesAsync();
                _logger.Info($"Successfully Added Subscription with Id: {newSub.SubscriptionId}");
                return true;
            }
            _logger.Warn($"Table already contains Subscription with Id: {newSub.SubscriptionId}");
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
                _logger.Info($"Successfully Updated Subscription with Id: {updateSub.SubscriptionId}");
                return true;
            }
            _logger.Warn($"Table does not contain Subscription with Id: {updateSub.SubscriptionId}");
            return false;
        }
        public async Task<bool> DeleteSubscription(string subscriptionId)
        {
            var checkSub = await _context.Subscriptions.FirstOrDefaultAsync(u => u.SubscriptionId == subscriptionId);
            if (checkSub != null)
            {
                _context.Remove(checkSub);
                await _context.SaveChangesAsync();
                _logger.Info($"Successfully Deleted Subscription with Id: {subscriptionId}");
                return true;
            }
            _logger.Warn($"Table does not contain Subscription with Id: {subscriptionId}");
            return false;
        }
    }
}