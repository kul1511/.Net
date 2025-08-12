using CROPDEAL.Models;
using CROPDEAL.Data;
using CROPDEAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using CROPDEAL.Models.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CROPDEAL.Repository
{
    public class CropRepository : ICrops
    {
        private readonly IMapper mapper;
        private readonly CropDealDbContext _crops;
        public CropRepository(CropDealDbContext user, IMapper _mapper)
        {
            _crops = user;
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
            await _crops.Logs.AddAsync(log);
            await _crops.SaveChangesAsync();
        }

        public async Task<IEnumerable<Crop>> GetAllCrops()
        {
            // try
            // {
            await LogToDatabase("Info", $"Getting All Crops Details....", DateTime.Now);
            var res = await _crops.Crops.Include(c => c.User).ToListAsync();
            if (res != null)
            {
                await LogToDatabase("Success", $"Successfully Retrieved Data", DateTime.Now);
                return res;
            }
            await LogToDatabase("Info", $"Table does not contain any Data", DateTime.Now);
            return res;

        }

        public async Task<Crop> GetCropById(string cropId)
        {
            // try
            // {
            var res = await _crops.Crops.Include(c => c.User).FirstOrDefaultAsync(u => u.CropId == cropId);
            if (res != null)
            {
                await LogToDatabase("Success", $"Successfully Retrieved Crop : {res}", DateTime.Now);
                return res;
            }
            await LogToDatabase("Info", $"Table does not contain any Crop with Id: {cropId}", DateTime.Now);
            return res;
            // }
        }
        public async Task<bool> AddCrop(CropDTO newCrop)
        {

            if (newCrop.Quantity < 0 || newCrop.Quantity == 0)
            {
                await LogToDatabase("Failed", "Crop Quantity cannot be less than or equal to 0.", DateTime.Now);
                return false;
            }
            if (newCrop.PricePerUnit < 0 || newCrop.PricePerUnit == 0)
            {
                await LogToDatabase("Failed", "Per Unit Price cannot be less than or equal to 0.", DateTime.Now);
                return false;
            }

            var userRole = await _crops.Users.FirstOrDefaultAsync(u => u.UserId == newCrop.UserId);

            if (userRole.Role == UserRole.Dealer)
            {
                await LogToDatabase("Failed", "Trying to Add Crop to a Dealer.", DateTime.Now);
                return false;
            }
            if (userRole.Role == UserRole.Admin)
            {
                await LogToDatabase("Failed", "Trying to Add Crop to a Admin, which is not possible", DateTime.Now);
                return false;
            }

            var crop = mapper.Map<Crop>(newCrop);

            var checkCrop = await _crops.Crops.FirstOrDefaultAsync(u => u.CropId == newCrop.CropId);
            if (checkCrop == null)
            {
                await _crops.Crops.AddAsync(crop);
                await _crops.SaveChangesAsync();
                await LogToDatabase("Success", $"Successfully Added Crop with Id: {newCrop.CropId}", DateTime.Now);
                return true;
            }
            await LogToDatabase("Info", $"Table already contains Crop with Id: {newCrop.CropId}", DateTime.Now);
            return false;
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
                await LogToDatabase("Success", $"Successfully Updated Crop with Id: {updateCrop.CropId}", DateTime.Now);
                return true;
            }
            await LogToDatabase("Info", $"Table does not contains Crop with Id: {updateCrop.CropId}", DateTime.Now);
            return false;
        }

        public async Task<bool> DeleteCrop(string cropId)
        {
            var checkCrop = await _crops.Crops.FirstOrDefaultAsync(u => u.CropId == cropId);
            if (checkCrop != null)
            {
                _crops.Remove(checkCrop);
                await _crops.SaveChangesAsync();
                await LogToDatabase("Success", $"Successfully Deleted Crop with Id: {cropId}", DateTime.Now);
                return true;
            }
            await LogToDatabase("Info", $"Table does not contains Crop with Id: {cropId}", DateTime.Now);
            return false;
        }

        public async Task<IEnumerable<Crop>> GetCropsByUserId(string userId)
        {
            var userRole = await _crops.Users.FirstOrDefaultAsync(o => o.UserId == userId);

            if (userRole.Role != UserRole.Farmer)
            {
                await LogToDatabase("Failed", "Trying to get Orders for Dealer or Admin which is not allowed", DateTime.Now);
                return null;
            }

            var cropByUserId = await _crops.Crops.Where(c => c.UserId == userId).ToListAsync();

            if (cropByUserId != null)
            {
                await LogToDatabase("Success", $"Recieved {cropByUserId.Count} crops for Farmer with Id: {userId}", DateTime.Now);
                return cropByUserId;
            }
            await LogToDatabase("Info", $"Table does not contains Crop for Farmer with Id: {userId}", DateTime.Now);
            return null;
        }
    }
}