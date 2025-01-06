using N_Tier.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Tier.Application.Services;

public interface IPaymentService
{
    Task<PaymentResponseDTO> MakePayment(MakePaymentDTO paymentDto);
    Task<PaymentResponseDTO> TopUpBalance(TopUpBalanceDTO topUpDto);
}
