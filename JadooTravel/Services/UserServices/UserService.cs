using Humanizer;
using JadooTravel.Dtos.UserDtos;
using JadooTravel.Entities;
using JadooTravel.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using MongoDB.Driver;
using System.Security.Claims;

namespace JadooTravel.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _userCollection;
        public UserService(IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _userCollection = database.GetCollection<User>(databaseSettings.UserCollectionName);
        }


        public async Task CreateUserAsync(CreateUserDto createUserDto)
        {
            var user = new User
            {
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                UserName = createUserDto.UserName,
                Password = createUserDto.Password,
                ImageUrl = string.IsNullOrEmpty(createUserDto.ImageUrl)
                ? "/Admin/assets/images/defaultuser.png"
                : createUserDto.ImageUrl
            };

            await _userCollection.InsertOneAsync(user);
        }


        public async Task<ResultUserDto?> GetUserAsync(string userId)
        {
            var user = await _userCollection.Find(x => x.UserId == userId).FirstOrDefaultAsync();

            if (user == null)
                return null;

            return new ResultUserDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                ImageUrl = string.IsNullOrEmpty(user.ImageUrl)
                    ? "/Admin/assets/images/defaultuser.png"
                    : user.ImageUrl
            };
        }

        public async Task<GetUserByIdDto> GetUserByIdAsync(string userId)
        {
            var value = await _userCollection.Find(x => x.UserId == userId).FirstOrDefaultAsync();
            return new GetUserByIdDto
            {
                UserId =value.UserId,
                FirstName = value.FirstName,
                LastName = value.LastName,
                UserName = value.UserName,
                ImageUrl = value.ImageUrl
            };

        }

        public async Task<ResultUserDto?> LoginUserAsync(LoginUserDto loginUserDto)
        {
            var user = await _userCollection.Find(x =>
                x.UserName == loginUserDto.UserName &&
                x.Password == loginUserDto.Password
            ).FirstOrDefaultAsync();

            if (user == null)
                return null;

            return new ResultUserDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ImageUrl = user.ImageUrl
            };
        }


        public async Task UpdatePasswordAsync(UpdateUserPasswordDto updateUserPasswordDto)
        {
            var user = await _userCollection.Find(x => x.UserId == updateUserPasswordDto.UserId).FirstOrDefaultAsync();

            if (user.Password != updateUserPasswordDto.CurrentPassword)
                throw new Exception("Mevcut şifre hatalı!");

            if (updateUserPasswordDto.NewPassword != updateUserPasswordDto.ConfirmNewPassword)
                throw new Exception("Yeni şifreler eşleşmiyor!");

            user.Password = updateUserPasswordDto.NewPassword;

            await _userCollection.ReplaceOneAsync(x => x.UserId == user.UserId, user);
        }
        public async Task UpdateUserAsync(UpdateUserDto updateUserDto)
        {
            var user = await _userCollection.Find(x => x.UserId == updateUserDto.UserId).FirstOrDefaultAsync();

            user.FirstName = updateUserDto.FirstName;
            user.LastName = updateUserDto.LastName;
            user.UserName = updateUserDto.UserName;
            user.ImageUrl = updateUserDto.ImageUrl;

            await _userCollection.ReplaceOneAsync(x => x.UserId == user.UserId, user);
        }
    }
}
