namespace N_Tier.Application.Services;

public interface INotificationService
{
    Task SendLowBalanceNotification(Guid userId, int currentBalance, int requiredAmount);
}
