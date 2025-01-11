using N_Tier.DataAccess.Repositories;

namespace N_Tier.Application.Services.Impl;

public class NotificationService : INotificationService
{
    private readonly IUserRepository _usersRepository;
    // Add email service, SMS service, or other notification mechanisms as needed

    public NotificationService(IUserRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task SendLowBalanceNotification(Guid userId, int currentBalance, int requiredAmount)
    {
        var user = await _usersRepository.GetFirstAsync(u => u.Id == userId);

        // Here you would implement your actual notification logic
        // For example, sending an email:
        var message = $"Dear {user.Name}, your account balance ({currentBalance}) is lower than " +
                     $"the required amount ({requiredAmount}) for your monthly tariff payment. " +
                     "Please top up your balance to avoid service interruption.";

        // Send email, SMS, or other notification
        // await _emailService.SendEmail(user.Email, "Low Balance Warning", message);
    }
}
