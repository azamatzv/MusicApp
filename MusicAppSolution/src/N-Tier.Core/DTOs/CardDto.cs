using N_Tier.Core.Entities;

namespace N_Tier.Core.DTOs;

public class CardDto
{
    public required int CardNumber { get; set; }

    public required string Expire_Date { get; set; }

    public CardType CardType { get; set; }
}