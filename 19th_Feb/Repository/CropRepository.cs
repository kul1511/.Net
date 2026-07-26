using CROPDEAL.Models;
using CROPDEAL.Data;
using CROPDEAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using CROPDEAL.Models.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using CROPDEAL.Services;
using log4net;

namespace CROPDEAL.Repository
{
    public class CropRepository : ICrops
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(CropRepository));
        private readonly IMapper mapper;
        private readonly CropDealDbContext _crops;
        public CropRepository(CropDealDbContext user, IMapper _mapper)
        {
            _crops = user;
            mapper = _mapper;
        }

        public async Task<IEnumerable<CropDealerViewDTO>> GetAllCrops()
        {
            _logger.Info($"Getting All Crops Details....");
            var res = await _crops.Crops
                .Include(c => c.User)
                .Where(c => c.User!.Role == UserRole.Farmer)
                .Select(c => new CropDealerViewDTO
                {
                    CropId = c.CropId,
                    CropType = c.CropType,
                    Quantity = c.Quantity,
                    PricePerUnit = c.PricePerUnit,
                    Location = c.Location,
                    FarmerName = c.User!.FullName
                })
                .ToListAsync();
            if (res != null)
            {
                _logger.Info($"Successfully Retrieved Data");
                return res;
            }
            _logger.Info($"Table does not contain any Data");
            return res!;
        }

        public async Task<CropDealerViewDTO> GetCropById(string cropId)
        {
            var res = await _crops.Crops
                .Include(c => c.User)
                .Where(c => c.User!.Role == UserRole.Farmer)
                .Select(c => new CropDealerViewDTO
                {
                    CropId = c.CropId,
                    CropType = c.CropType,
                    Quantity = c.Quantity,
                    PricePerUnit = c.PricePerUnit,
                    Location = c.Location,
                    FarmerName = c.User!.FullName
                })
                .ToListAsync();
            if (res != null)
            {
                _logger.Info($"Successfully Retrieved Crop : {res}");
                return res[0];
            }
            _logger.Info($"Table does not contain any Crop with Id: {cropId}");
            return res![0];
        }
        public async Task<bool> AddCrop(CropDTO newCrop)
        {

            if (newCrop.Quantity < 0 || newCrop.Quantity == 0)
            {
                _logger.Warn($"Crop Quantity cannot be less than or equal to 0.");
                return false;
            }
            if (newCrop.PricePerUnit < 0 || newCrop.PricePerUnit == 0)
            {
                _logger.Warn($"Per Unit Price cannot be less than or equal to 0.");
                return false;
            }

            var userRole = await _crops.Users.FirstOrDefaultAsync(u => u.UserId == newCrop.UserId);

            if (userRole!.Role == UserRole.Dealer)
            {
                _logger.Warn($"Trying to Add Crop to a Dealer.");
                return false;
            }
            if (userRole.Role == UserRole.Admin)
            {
                _logger.Warn($"Trying to Add Crop to a Admin, which is not possible");
                return false;
            }

            var crop = mapper.Map<Crop>(newCrop);
            crop.CropId = String.Concat("CRP", crop.CropType!.Substring(0, 1), new Random().Next(100, 1000));

            var checkCrop = await _crops.Crops.FirstOrDefaultAsync(u => u.CropId == newCrop.CropId);
            if (checkCrop == null)
            {
                await _crops.Crops.AddAsync(crop);
                await _crops.SaveChangesAsync();
                _logger.Info($"Successfully Added Crop with Id: {newCrop.CropId}");

                var cropAddedByFarmer = await _crops.Users.FirstOrDefaultAsync(u => u.UserId == newCrop.UserId);

                if (cropAddedByFarmer != null)
                {
                    List<string> dealerEmails = GetDealerEmails();
                    var emailNotify = new EmailNotification();
                    await emailNotify.SendCropNotificationAsync(cropAddedByFarmer.FullName!, cropAddedByFarmer.Email!, dealerEmails, crop);
                }
                return true;
            }

            _logger.Info($"Table already contains Crop with Id: {newCrop.CropId}");
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
                _logger.Info($"Successfully Updated Crop with Id: {updateCrop.CropId}");
                return true;
            }
            _logger.Info($"Table does not contains Crop with Id: {updateCrop.CropId}");
            return false;
        }

        public async Task<bool> DeleteCrop(string cropId)
        {
            var checkCrop = await _crops.Crops.FirstOrDefaultAsync(u => u.CropId == cropId);
            if (checkCrop != null)
            {
                _crops.Remove(checkCrop);
                await _crops.SaveChangesAsync();
                _logger.Info($"Successfully Deleted Crop with Id: {cropId}");
                return true;
            }
            _logger.Info($"Table does not contains Crop with Id: {cropId}");
            return false;
        }

        public async Task<IEnumerable<Crop>> GetCropsByUserId(string userId)
        {
            var userRole = await _crops.Users.FirstOrDefaultAsync(o => o.UserId == userId);

            if (userRole!.Role != UserRole.Farmer)
            {
                _logger.Warn($"Trying to get Orders for Dealer or Admin which is not allowed");
                return null!;
            }

            var cropByUserId = await _crops.Crops.Where(c => c.UserId == userId).ToListAsync();

            if (cropByUserId != null)
            {
                _logger.Info($"Recieved {cropByUserId.Count} crops for Farmer with Id: {userId}");
                return cropByUserId;
            }
            _logger.Info($"Table does not contains Crop for Farmer with Id: {userId}");
            return null!;
        }

        public List<string> GetDealerEmails()
        {
            var users = _crops.Users.Where(d => d.Role == UserRole.Dealer);
            List<string> res = new List<string>();
            foreach (var v in users)
            {
                res.Add(v.Email!);
            }
            return res;
        }
    }
}