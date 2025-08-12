using CROPDEAL.Models;
using CROPDEAL.Models.DTO;

namespace CROPDEAL.Interfaces
{
    public interface ICrops
    {
        Task LogToDatabase(string level, string message, DateTime now);
        Task<IEnumerable<Crop>> GetAllCrops();
        Task<Crop> GetCropById(string cropId);
        Task<bool> AddCrop(CropDTO crop);
        Task<bool> UpdateCrop(CropDTO crop);
        Task<bool> DeleteCrop(string cropId);
        Task<IEnumerable<Crop>> GetCropsByUserId(string userId);
    }
}