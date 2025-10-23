using JadooTravel.Dtos.CategoryDtos;
using JadooTravel.Dtos.FeatureDtos;

namespace JadooTravel.Services.FeatureServices
{
    public interface IFeatureService
    {
        Task<ResultFeatureDto> GetFeatureAsync();
        Task<GetFeatureByIdDto> GetFeatureByIdAsync(string id);
        Task UpdateFeatureAsync(UpdateFeatureDto updateFeatureDto);
        Task CreateFeatureAsync(CreateFeatureDto createFeatureDto);
    }
}
