using AutoMapper;
using JadooTravel.Dtos.TripPlanDtos;
using JadooTravel.Entities;
using JadooTravel.Settings;
using MongoDB.Driver;

namespace JadooTravel.Services.TripPlanServices
{
	public class TripPlanService : ITripPlanService
	{
		private readonly IMongoCollection<TripPlan> _tripCollection;
		private readonly IMapper _mapper;

		public TripPlanService(IMapper mapper, IDatabaseSettings _databaseSettings)
		{
			var client = new MongoClient(_databaseSettings.ConnectionString);
			var database = client.GetDatabase(_databaseSettings.DatabaseName);
			_tripCollection = database.GetCollection<TripPlan>(_databaseSettings.TripPlanCollectionName);
			_mapper = mapper;
		}

		public async Task UpdateTripPlanAsync(UpdateTripPlanDto dto)
		{
			var existing = await _tripCollection.Find(x => x.TripPlanId == dto.TripPlanId).FirstOrDefaultAsync();
			if (existing == null) return;

			// Boş bırakılan alanlar eski değerini korur
			var update = Builders<TripPlan>.Update
				.Set(x => x.Title, dto.Title ?? existing.Title)
				.Set(x => x.Description, dto.Description ?? existing.Description)
				.Set(x => x.ImageUrl, string.IsNullOrEmpty(dto.ImageUrl) ? existing.ImageUrl : dto.ImageUrl);

			await _tripCollection.UpdateOneAsync(x => x.TripPlanId == dto.TripPlanId, update);
		}

		public async Task<GetTripPlanByIdDto> GetTripPlanByIdAsync(string id)
		{
			var value = await _tripCollection.Find(x => x.TripPlanId == id).FirstOrDefaultAsync();
			return _mapper.Map<GetTripPlanByIdDto>(value);
		}

		public async Task<List<ResultTripPlanDto>> GetAllTripPlanAsync()
		{
			var values = await _tripCollection.Find(x => true).ToListAsync();
			return _mapper.Map<List<ResultTripPlanDto>>(values);
		}

		public Task CreateTripPlanAsync(CreateTripPlanDto createTripPlanDto)
		{
			throw new NotImplementedException();
		}

		public Task DeleteTripPlanAsync(string id)
		{
			throw new NotImplementedException();
		}

		// Diğer CRUD metodlarını da buraya ekleyebilirsin
	}
}
