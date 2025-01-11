using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Tier.Core.DTOs.PaymentDtos;

public class PaymentHistoryDTO
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public string AccountName { get; set; }
    public decimal Amount { get; set; }
    public string TariffName { get; set; }
    public DateTime PaymentMonth { get; set; }
    public DateTime CreatedOn { get; set; }
    public bool IsPaid { get; set; }
}
