using CROPDEAL.Models;
using CROPDEAL.Data;
using CROPDEAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using CROPDEAL.Models.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using CROPDEAL.Services;
using System.Runtime.CompilerServices;

namespace CROPDEAL.Repository
{
    public class CropRepository : ICrops
    {
        ILogger<CropRepository> log;
        private readonly IMapper mapper;
        private readonly CropDealDbContext _crops;
        public CropRepository(CropDealDbContext user, IMapper _mapper, ILogger<CropRepository> llog)
        {
            _crops = user;
            mapper = _mapper;
            log = llog;
        }

        public async Task LogToDatabase(string level, string message, DateTime now)
        {
            var log = new Log
            {
                LogLevel = level,
                Message = message,
                Timestamp = now
            };
            await _crops.Logs.AddAsync(log);
            await _crops.SaveChangesAsync();
        }

        public async Task<IEnumerable<CropDealerViewDTO>> GetAllCrops()
        {
            // try
            // {
            log.LogInformation($"Getting All Crops Details....", DateTime.Now);
            var res = await _crops.Crops
                .Include(c => c.User)
                .Where(c => c.User.Role == UserRole.Farmer)
                .Select(c => new CropDealerViewDTO
                {
                    CropId = c.CropId,
                    CropType = c.CropType,
                    Quantity = c.Quantity,
                    PricePerUnit = c.PricePerUnit,
                    Location = c.Location,
                    FarmerName = c.User.FullName
                })
                .ToListAsync();
            if (res != null)
            {
                log.LogInformation($"Successfully Retrieved Data", DateTime.Now);
                return res;
            }
            log.LogInformation($"Table does not contain any Data", DateTime.Now);
            return res;

        }

        public async Task<CropDealerViewDTO> GetCropById(string cropId)
        {
            // try
            // {
            var res = await _crops.Crops
        .Include(c => c.User)
        .Where(c => c.CropId == cropId && c.User.Role == UserRole.Farmer)
        .Select(c => new CropDealerViewDTO
        {
            CropId = c.CropId,
            CropType = c.CropType,
            Quantity = c.Quantity,
            PricePerUnit = c.PricePerUnit,
            Location = c.Location,
            FarmerName = c.User.FullName
        })
        .FirstOrDefaultAsync();

            if (res != null)
            {
                log.LogInformation($"Successfully Retrieved Crop : {res}", DateTime.Now);
                return res;
            }
            log.LogInformation($"Table does not contain any Crop with Id: {cropId}", DateTime.Now);
            return res;
            // }
        }


        public async Task<bool> AddCrop(CropDTO newCrop)
        {

            if (newCrop.Quantity < 0 || newCrop.Quantity == 0)
            {
                log.LogError("Crop Quantity cannot be less than or equal to 0.", DateTime.Now);
                return false;
            }
            if (newCrop.PricePerUnit < 0 || newCrop.PricePerUnit == 0)
            {
                log.LogError("Per Unit Price cannot be less than or equal to 0.", DateTime.Now);
                return false;
            }

            var userRole = await _crops.Users.FirstOrDefaultAsync(u => u.UserId == newCrop.UserId);

            if (userRole.Role == UserRole.Dealer)
            {
                log.LogError("Trying to Add Crop to a Dealer.", DateTime.Now);
                return false;
            }
            if (userRole.Role == UserRole.Admin)
            {
                log.LogError("Trying to Add Crop to a Admin, which is not possible", DateTime.Now);
                return false;
            }


            var crop = mapper.Map<Crop>(newCrop);

            // crop.UserId = _

            var checkCrop = await _crops.Crops.FirstOrDefaultAsync(u => u.CropId == newCrop.CropId);
            if (checkCrop == null)
            {
                await _crops.Crops.AddAsync(crop);
                await _crops.SaveChangesAsync();
                log.LogInformation($"Successfully Added Crop with Id: {newCrop.CropId}", DateTime.Now);

                var cropAddedByFarmer = await _crops.Users.FirstOrDefaultAsync(u => u.UserId == newCrop.UserId);

                //Sending Notification Email to All Dealers
                List<string> dealerEmails = GetDealerEmails();
                var emailNotify = new EmailNotification();
                await emailNotify.SendCropNotificationAsync(cropAddedByFarmer.FullName, cropAddedByFarmer.Email, dealerEmails, crop);

                return true;
            }
            log.LogInformation($"Table already contains Crop with Id: {newCrop.CropId}", DateTime.Now);
            return false;
        }


        public List<string> GetDealerEmails()
        {
            var users = _crops.Users.Where(d => d.Role == UserRole.Dealer);
            List<string> res = new List<string>();
            foreach (var v in users)
            {
                res.Add(v.Email);
            }
            return res;
        }

        public async Task<bool> UpdateCrop(CropDTO updateCrop)
        {
            var crop = mapper.Map<Crop>(updateCrop);
            var checkCrop = await _crops.Crops.FirstOrDefaultAsync(u => u.CropId == updateCrop.CropId);
            if (checkCrop != null)
            {
                checkCrop.CropType = crop.CropType;
                checkCrop.Quantity = crop.Quantity;
                checkCrop.PricePerUnit = crop.PricePerUnit;
                checkCrop.Location = crop.Location;
                await _crops.SaveChangesAsync();
                log.LogInformation($"Successfully Updated Crop with Id: {updateCrop.CropId}", DateTime.Now);
                return true;
            }
            log.LogInformation($"Table does not contains Crop with Id: {updateCrop.CropId}", DateTime.Now);
            return false;
        }

        public async Task<bool> DeleteCrop(string cropId)
        {
            var checkCrop = await _crops.Crops.FirstOrDefaultAsync(u => u.CropId == cropId);
            if (checkCrop != null)
            {
                _crops.Remove(checkCrop);
                await _crops.SaveChangesAsync();
                log.LogInformation($"Successfully Deleted Crop with Id: {cropId}", DateTime.Now);
                return true;
            }
            log.LogInformation($"Table does not contains Crop with Id: {cropId}", DateTime.Now);
            return false;
        }

        public async Task<IEnumerable<CropDealerViewDTO>> GetCropsByUserId(int userId)
        {
            var res = await _crops.Crops
        .Include(c => c.User)
        .Where(c => c.UserId == userId && c.User.Role == UserRole.Farmer)
        .Select(c => new CropDealerViewDTO
        {
            CropId = c.CropId,
            CropType = c.CropType,
            Quantity = c.Quantity,
            PricePerUnit = c.PricePerUnit,
            Location = c.Location,
            FarmerName = c.User.FullName
        })
        .ToListAsync();

            if (res != null)
            {
                log.LogInformation($"Recieved {res.Count} crops for Farmer with Id: {userId}", DateTime.Now);
                return res;
            }
            log.LogInformation($"Table does not contains Crop for Farmer with Id: {userId}", DateTime.Now);
            return null;
        }
    }
}