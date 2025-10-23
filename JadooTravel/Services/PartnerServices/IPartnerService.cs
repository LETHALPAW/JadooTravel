using JadooTravel.Dtos.PartnerDtos;

namespace JadooTravel.Services.PartnerServices
{
    public interface IPartnerService
    {
        Task<List<ResultPartnerDto>> GetAllPartnerAsync();
        Task CreatePartnerAsync(CreatePartnerDto createPartnerDto);
        Task UpdatePartnerAsync(UpdatePartnerDto updatePartnerDto);
        Task<GetPartnerByIdDto> GetPartnerByIdAsync(string id);
        Task DeletePartnerAsync(string id);
    }
}
