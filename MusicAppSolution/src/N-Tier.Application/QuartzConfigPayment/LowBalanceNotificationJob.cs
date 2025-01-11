using N_Tier.Application.Services;
using N_Tier.DataAccess.Repositories;
using Quartz;

namespace N_Tier.Application.QuartzConfigPayment
{
    [DisallowConcurrentExecution]
    public class LowBalanceNotificationJob : IJob
    {
        private readonly IPaymentService _paymentService;
        private readonly IAccountsRepository _accountsRepository;

        public LowBalanceNotificationJob(
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
                await _paymentService.CheckAndNotifyLowBalance(account.Id);
            }
        }
    }
}
