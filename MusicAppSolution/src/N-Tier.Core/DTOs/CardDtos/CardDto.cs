using System.Text.Json.Serialization;

namespace N_Tier.Core.DTOs.CardDtos
{
    public class CardDto
    {
        [JsonIgnore]
        public Guid UserId { get; set; }

        public long CardNumber { get; set; }


        //[JsonConverter(typeof(CardExpiryDateConverter))]
        public DateTime Expire_Date { get; set; }
    }
}
