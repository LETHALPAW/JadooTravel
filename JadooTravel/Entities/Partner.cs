using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace JadooTravel.Entities
{
    public class Partner
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string PartnerId { get; set; }   
        public string PartnerName { get; set; }   
        public string LogoUrl { get; set; }   
        public bool Status { get; set; }      
    }
}
