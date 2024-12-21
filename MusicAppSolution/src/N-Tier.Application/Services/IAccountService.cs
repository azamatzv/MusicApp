using N_Tier.Core.DTOs;

namespace N_Tier.Application.Services;

public interface IAccountService
{
    Task<AccountDto> GetByIdAsync(Guid id);
    Task<List<AccountDto>> GetAllAsync();
    Task<AccountDto> AddAccountAsync(AccountDto accountDto);
    Task<AccountDto> UpdateAccountAsync(Guid id, AccountDto accountDto);
    Task<bool> DeleteAccountAsync(Guid id);
}
