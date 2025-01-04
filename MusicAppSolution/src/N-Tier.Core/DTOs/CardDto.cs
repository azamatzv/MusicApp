namespace N_Tier.Core.DTOs
{
    public class CardDto
    {
        public Guid UserId { get; set; }

        public int CardNumber { get; set; }

        public string Expire_Date { get; set; }
    }
}
