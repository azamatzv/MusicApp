namespace N_Tier.Core.DTOs;

public class TopUpBalanceDTO
{
    public Guid UserId { get; set; }
    public Guid CardId { get; set; }
    public int Amount { get; set; }
}
