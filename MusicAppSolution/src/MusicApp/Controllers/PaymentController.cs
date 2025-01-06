using Microsoft.AspNetCore.Mvc;
using N_Tier.Application.Services;
using N_Tier.Core.DTOs;
using N_Tier.Core.Exceptions;

namespace MusicApp.Controllers
{
    public class PaymentController : ApiController
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("make-payment")]
        public async Task<ActionResult<PaymentResponseDTO>> MakePayment([FromBody] MakePaymentDTO paymentDto)
        {
            try
            {
                var result = await _paymentService.MakePayment(paymentDto);
                if (!result.Success)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while processing the payment" });
            }
        }

        [HttpPost("top-up-balance")]
        public async Task<ActionResult<PaymentResponseDTO>> TopUpBalance([FromBody] TopUpBalanceDTO topUpDto)
        {
            try
            {
                var result = await _paymentService.TopUpBalance(topUpDto);
                return Ok(result);
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while topping up the balance" });
            }
        }
    }
}
