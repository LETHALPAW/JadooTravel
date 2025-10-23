using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JadooTravel.Entities
{
    public class Testimonial
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string TestimonialId { get; set; }
        public string FullName { get; set; }
        public string Comment { get; set; }
        public string ImageURL { get; set; }
        public string City { get; set; }
        public string Country{ get; set; }

    }
}
