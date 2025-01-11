using N_Tier.Application.Services;
using N_Tier.DataAccess.Repositories;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Tier.Application.QuartzConfigPayment;
[DisallowConcurrentExecution]
public class MonthlyPaymentJob : IJob
{
    private readonly IPaymentService _paymentService;
    private readonly IAccountsRepository _accountsRepository;

    public MonthlyPaymentJob(
        IPaymentService paymentService,
        IAccountsRepository accountsRepository)
    {
        _paymentService = paymentService;
        _accountsRepository = accountsRepository;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var accounts = await _accountsRepository.GetAllAsync(a => !a.IsDeleted);

        foreach (var account in accounts)
        {
            try
            {
                await _paymentService.ProcessAutomaticMonthlyPayment(account.Id);
            }
            catch (Exception ex)
            {
                // Xatolik logi
                Console.WriteLine($"Error processing payment for account {account.Id}: {ex.Message}");
            }
        }
    }
}


