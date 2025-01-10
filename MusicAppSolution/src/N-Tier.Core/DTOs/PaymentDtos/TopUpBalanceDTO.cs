using System.Text.Json.Serialization;

namespace N_Tier.Core.DTOs.PaymentDtos;

public class TopUpBalanceDTO
{
    [JsonIgnore]
    public Guid UserId { get; set; }

    public Guid CardId { get; set; }
    public int Amount { get; set; }
}
