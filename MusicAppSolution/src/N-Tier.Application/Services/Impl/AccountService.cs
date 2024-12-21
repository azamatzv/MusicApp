using N_Tier.Core.DTOs;
using N_Tier.Core.Entities;
using N_Tier.DataAccess.Repositories;

namespace N_Tier.Application.Services.Impl;

public class AccountService
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
        var account = new Accounts
        {
            Name = accountDto.Name,
            TariffType = accountDto.TariffType,
            Balance = accountDto.Balance,
            UserId = accountDto.UserId,
        };

        await _accountRepository.AddAsync(account);
        return MapToDto(account);
    }

    public async Task<AccountDto> UpdateAccountAsync(Guid id, AccountDto accountDto)
    {
        var account = await _accountRepository.GetFirstAsync(a => a.Id == id);
        if (account == null) throw new Exception("Account not found");

        account.Name = accountDto.Name;
        account.TariffType = accountDto.TariffType;
        account.Balance = accountDto.Balance;

        await _accountRepository.UpdateAsync(account);
        
        return MapToDto(account);
    }

    public async Task<bool> DeleteAccountAsync(Guid id)
    {
        var account = await _accountRepository.GetFirstAsync(a => a.Id == id);
        if (account == null) throw new Exception("Account not found");

        await _accountRepository.DeleteAsync(account);
        return true;
    }

    private AccountDto MapToDto(Accounts account)
    {
        return new AccountDto
        {
            Name = account.Name,
            TariffType = account.TariffType,
            Balance = account.Balance,
            UserId = account.UserId
        };
    }
}
