using N_Tier.Core.DTOs;
using N_Tier.Core.Entities;
using N_Tier.DataAccess.Repositories;

namespace N_Tier.Application.Services.Impl;

public class AccountService : IAccountService
{
    private readonly IAccountsRepository _accountRepository;

    public AccountService(IAccountsRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<AccountDto> GetByIdAsync(Guid id)
    {
        var account = await _accountRepository.GetFirstAsync(a => a.Id == id);
        if (account == null) throw new Exception("Account not found");

        return MapToDto(account);
    }

    public async Task<List<AccountDto>> GetAllAsync()
    {
        var accounts = await _accountRepository.GetAllAsync(_ => true);
        return accounts.Select(MapToDto).ToList();
    }

    public async Task<AccountDto> AddAccountAsync(AccountDto accountDto)
    {
        var existingAccount = await _accountRepository.GetFirstAccountAsync(a => a.UserId == accountDto.UserId);
        if (existingAccount != null)
        {
            throw new Exception("An active account already exists for this UserId");
        }

        var account = new Accounts
        {
            Name = accountDto.Name,
            TariffTypeId = accountDto.TariffTypeId,
            Balance = accountDto.Balance,
            UserId = accountDto.UserId,
        };

        await _accountRepository.AddAsync(account);
        return MapToDto(account);
    }

    public async Task<AccountDto> UpdateAccountAsync(Guid id, UpdateAccountDto updateAccountDto)
    {

        var account = await _accountRepository.GetFirstAsync(a => a.Id == id);
        if (account == null) throw new Exception("Account not found");

        if (!string.IsNullOrEmpty(updateAccountDto.Name))
        {
            account.Name = updateAccountDto.Name;
        }

        if (updateAccountDto.TariffTypeId.HasValue)
        {
            account.TariffTypeId = updateAccountDto.TariffTypeId.Value;
        }

        await _accountRepository.UpdateAsync(account);

        return MapToDto(account);
    }

    public async Task<bool> DeleteAccountAsync(Guid id)
    {
        var account = await _accountRepository.GetFirstAccountAsync(a => a.Id == id && !a.IsDeleted);
        if (account == null) throw new KeyNotFoundException("Account not found");

        // Soft delete: faqat IsDeleted maydonini true qilib belgilaymiz
        account.IsDeleted = true;
        account.UpdatedOn = DateTime.UtcNow; // Yangilangan vaqtni belgilang

        await _accountRepository.UpdateAsync(account); // Yangilangan hisobni saqlaymiz
        return true;
    }

    private AccountDto MapToDto(Accounts account)
    {
        return new AccountDto
        {
            Name = account.Name,
            TariffTypeId = account.TariffTypeId,
            Balance = account.Balance,
            UserId = account.UserId
        };
    }
}
