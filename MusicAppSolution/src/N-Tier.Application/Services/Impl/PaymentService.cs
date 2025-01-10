using N_Tier.Core.DTOs.PaymentDtos;
using N_Tier.Core.Entities;
using N_Tier.Core.Exceptions;
using N_Tier.DataAccess.Repositories;

namespace N_Tier.Application.Services.Impl;

public class PaymentService : IPaymentService
{
    private readonly IAccountsRepository _accountsRepository;
    private readonly IPaymentHistoryRepository _paymentHistoryRepository;
    private readonly ICardsRepository _cardsRepository;
    private readonly ITariffTypeRepository _tariffRepository;

    public PaymentService(
        IAccountsRepository accountsRepository,
        IPaymentHistoryRepository paymentHistoryRepository,
        ICardsRepository cardsRepository,
        ITariffTypeRepository tariffRepository)
    {
        _accountsRepository = accountsRepository;
        _paymentHistoryRepository = paymentHistoryRepository;
        _cardsRepository = cardsRepository;
        _tariffRepository = tariffRepository;
    }

    public async Task<PaymentResponseDTO> MakePayment(MakePaymentDTO paymentDto)
    {
        var account = await _accountsRepository.GetFirstAsync(a => a.Id == paymentDto.AccountId);
        var tariff = await _tariffRepository.GetFirstAsync(t => t.Id == account.TariffTypeId);

        // Get user's card
        var card = await _cardsRepository.GetFirstAsync(c => c.UserId == account.UserId);
        if (card == null)
        {
            throw new ResourceNotFoundException("No card found for this user");
        }

        if (account.Balance < tariff.Amount)
        {
            return new PaymentResponseDTO
            {
                Success = false,
                Message = "Insufficient balance",
                NewBalance = account.Balance
            };
        }

        account.Balance -= tariff.Amount;
        await _accountsRepository.UpdateAsync(account);

        var paymentHistory = new PaymentHistory
        {
            AccountsId = account.Id,
            CardsId = card.Id,
            TariffTypeId = tariff.Id,
            IsPaid = true,
            CreatedOn = DateTime.UtcNow
        };

        await _paymentHistoryRepository.AddAsync(paymentHistory);

        return new PaymentResponseDTO
        {
            Success = true,
            Message = "Payment successful",
            NewBalance = account.Balance
        };
    }

    public async Task<PaymentResponseDTO> TopUpBalance(TopUpBalanceDTO topUpDto)
    {
        var account = await _accountsRepository.GetFirstAsync(a => a.UserId == topUpDto.UserId);
        var card = await _cardsRepository.GetFirstAsync(c => c.Id == topUpDto.CardId && c.UserId == topUpDto.UserId);

        if (card == null)
        {
            throw new ResourceNotFoundException("Card not found or doesn't belong to the user");
        }

        account.Balance += topUpDto.Amount;
        await _accountsRepository.UpdateAsync(account);

        return new PaymentResponseDTO
        {
            Success = true,
            Message = "Balance topped up successfully",
            NewBalance = account.Balance
        };
    }
}
