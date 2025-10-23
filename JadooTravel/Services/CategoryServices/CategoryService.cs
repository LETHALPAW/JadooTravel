using AutoMapper;
using JadooTravel.Dtos.CategoryDtos;
using JadooTravel.Entities;
using JadooTravel.Settings;
using MongoDB.Driver;

namespace JadooTravel.Services.CategoryServices
{
	public class CategoryService : ICategoryService
	{
		private readonly IMongoCollection<Category> _categoryCollection;
		private readonly IMapper _mapper;

		public CategoryService(IDatabaseSettings databaseSettings, IMapper mapper)
		{
			var client = new MongoClient(databaseSettings.ConnectionString);
			var database = client.GetDatabase(databaseSettings.DatabaseName);
			_categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
			_mapper = mapper;
		}

		// Yeni kategori ekleme
		public async Task CreateCategoryAsync(CreateCategoryDto createCategoryDto)
		{
			var value = _mapper.Map<Category>(createCategoryDto);
			await _categoryCollection.InsertOneAsync(value);
		}

		// Kategori silme
		public async Task DeleteCategoryAsync(string id)
		{
			await _categoryCollection.DeleteOneAsync(x => x.CategoryId == id);
		}

		// Tüm kategorileri listeleme
		public async Task<List<ResultCategoryDto>> GetAllCategoryAsync()
		{
			var values = await _categoryCollection.Find(x => true).ToListAsync();
			return _mapper.Map<List<ResultCategoryDto>>(values);
		}

		// Id’ye göre kategori bulma
		public async Task<ResultCategoryDto> GetCategoryByIdAsync(string id)
		{
			var category = await _categoryCollection.Find(x => x.CategoryId == id).FirstOrDefaultAsync();
			return _mapper.Map<ResultCategoryDto>(category);
		}

		// Kategori güncelleme
		public async Task UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto)
		{
			var value = _mapper.Map<Category>(updateCategoryDto);
			await _categoryCollection.FindOneAndReplaceAsync(x => x.CategoryId == updateCategoryDto.CategoryId, value);
		}
	}
}
