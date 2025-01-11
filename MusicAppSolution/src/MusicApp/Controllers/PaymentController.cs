using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using N_Tier.Application.Services;
using N_Tier.Core.DTOs.PaymentDtos;
using N_Tier.Core.Exceptions;

namespace MusicApp.Controllers
{
    [Authorize(Policy = "User")]
    public class PaymentController : ApiControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IConfiguration _configuration;

        public PaymentController(IPaymentService paymentService, IConfiguration configuration)
        {
            _paymentService = paymentService;
            _configuration = configuration;
        }

        [HttpPost("make-payment")]
        public async Task<ActionResult<PaymentResponseDTO>> MakePayment()
        {
            try
            {
                var accountId = GetAccountIdFromToken();

                var paymentDto = new MakePaymentDTO
                {
                    AccountId = accountId
                };
                var result = await _paymentService.MakePayment(paymentDto);
                if (!result.Success)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
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
                var userId = GetUserIdFromToken();
                topUpDto.UserId = userId;
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

        [HttpGet("payment-history")]
        public async Task<ActionResult<IEnumerable<PaymentHistoryDTO>>> GetPaymentHistory()
        {
            try
            {
                var accountId = GetAccountIdFromToken();
                var history = await _paymentService.GetPaymentHistory(accountId);
                return Ok(history);
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving payment history" });
            }
        }

        // Test endpoints - only available in Development environment
        [HttpPost("test/trigger-monthly-payment")]
        //[Authorize(Policy = "Admin")]
        public async Task<ActionResult> TriggerMonthlyPayment()
        {
            //if (!_configuration.GetValue<bool>("AllowTestEndpoints"))
            //{
            //    return NotFound();
            //}

            try
            {
                var accountId = GetAccountIdFromToken();

                await _paymentService.ProcessAutomaticMonthlyPayment(accountId);
                return Ok(new { message = "Monthly payment processed successfully" });
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while processing test payment" });
            }
        }

        [HttpPost("test/trigger-low-balance-notification")]
        //[Authorize(Policy = "Admin")]
        public async Task<ActionResult> TriggerLowBalanceNotification()
        {
            //if (!_configuration.GetValue<bool>("AllowTestEndpoints"))
            //{
            //    return NotFound();
            //}

            try
            {
                var accountId = GetAccountIdFromToken();
                await _paymentService.CheckAndNotifyLowBalance(accountId);
                return Ok(new { message = "Low balance notification check completed" });
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while processing notification test" });
            }
        }
    }
}
