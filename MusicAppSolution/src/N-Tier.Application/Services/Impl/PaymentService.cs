using N_Tier.Application.QuartzConfigPayment;
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
    private readonly INotificationService _notificationService;

    public PaymentService(
        IAccountsRepository accountsRepository,
        IPaymentHistoryRepository paymentHistoryRepository,
        ICardsRepository cardsRepository,
        ITariffTypeRepository tariffRepository,
        INotificationService notificationService)
    {
        _accountsRepository = accountsRepository;
        _paymentHistoryRepository = paymentHistoryRepository;
        _cardsRepository = cardsRepository;
        _tariffRepository = tariffRepository;
        _notificationService = notificationService;
    }

    public async Task<PaymentResponseDTO> MakePayment(MakePaymentDTO paymentDto)
    {
        var account = await _accountsRepository.GetFirstAsync(a => a.Id == paymentDto.AccountId);
        var tariff = await _tariffRepository.GetFirstAsync(t => t.Id == account.TariffTypeId);

        // Check if payment already made for current month
        var currentMonth = DateTime.UtcNow.Date.StartOfMonth();
        var existingPayment = await _paymentHistoryRepository.GetFirstHistoryAsync(p =>
            p.AccountsId == account.Id &&
            p.PaymentMonth.Year == currentMonth.Year &&
            p.PaymentMonth.Month == currentMonth.Month);

        if (existingPayment != null)
        {
            return new PaymentResponseDTO
            {
                Success = false,
                Message = "Payment already made for this month",
                NewBalance = account.Balance
            };
        }

        //var card = await _cardsRepository.GetFirstAsync(c => c.UserId == account.UserId);
        //if (card == null)
        //{
        //    throw new ResourceNotFoundException("No card found for this user");
        //}

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
            TariffTypeId = tariff.Id,
            CardsId = Guid.Empty,
            IsPaid = true,
            CreatedOn = DateTime.UtcNow,
            PaymentMonth = currentMonth
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

    public async Task ProcessAutomaticMonthlyPayment(Guid accountId)
    {
        var account = await _accountsRepository.GetFirstAsync(a => a.Id == accountId);
        var tariff = await _tariffRepository.GetFirstAsync(t => t.Id == account.TariffTypeId);

        await MakePayment(new MakePaymentDTO { AccountId = accountId });
    }

    public async Task CheckAndNotifyLowBalance(Guid accountId)
    {
        var account = await _accountsRepository.GetFirstAsync(a => a.Id == accountId);
        var tariff = await _tariffRepository.GetFirstAsync(t => t.Id == account.TariffTypeId);

        if (account.Balance < tariff.Amount)
        {
            await _notificationService.SendLowBalanceNotification(
                account.UserId,
                account.Balance,
                tariff.Amount);
        }
    }

    public async Task<IEnumerable<PaymentHistoryDTO>> GetPaymentHistory(Guid accountId)
    {
        var account = await _accountsRepository.GetFirstAsync(a => a.Id == accountId);
        if (account == null)
        {
            throw new ResourceNotFoundException("Account not found");
        }

        var payments = await _paymentHistoryRepository.GetAllHistoryAsync(
            ph => ph.AccountsId == accountId,
            includeProperties: "Accounts,TariffType"
        );

        return payments.Select(ph => new PaymentHistoryDTO
        {
            Id = ph.Id,
            AccountId = ph.AccountsId,
            AccountName = ph.Accounts.Name,
            Amount = ph.TariffType.Amount,
            TariffName = ph.TariffType.Name,
            PaymentMonth = ph.PaymentMonth,
            CreatedOn = ph.CreatedOn ?? DateTime.UtcNow,
            IsPaid = ph.IsPaid
        })
        .OrderByDescending(ph => ph.PaymentMonth)
        .ToList();
    }
}
