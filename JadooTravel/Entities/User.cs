using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace JadooTravel.Entities
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? ImageUrl { get; set; }

    }
}




