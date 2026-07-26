using CROPDEAL.Models;
using CROPDEAL.Models.DTO;

namespace CROPDEAL.Interfaces
{
    public interface ICrops
    {
        Task<IEnumerable<CropDealerViewDTO>> GetAllCrops();
        Task<CropDealerViewDTO> GetCropById(string cropId);
        Task<bool> AddCrop(CropDTO crop);
        Task<bool> UpdateCrop(CropDTO crop);
        Task<bool> DeleteCrop(string cropId);
        Task<IEnumerable<Crop>> GetCropsByUserId(string userId);
    }
}