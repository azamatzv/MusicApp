using Microsoft.EntityFrameworkCore;
using N_Tier.Core.Entities;
using N_Tier.Core.Exceptions;
using N_Tier.DataAccess.Persistence;

namespace N_Tier.DataAccess.Repositories.Impl;

public class OtpRepository : BaseRepository<OtpVerification>, IOtpRepository
{
    public OtpRepository(DatabaseContext context) : base(context) { }

    public async Task<OtpVerification> GetActiveOtpAsync(Guid userId, string otpCode)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("Invalid user ID", nameof(userId));

        if (string.IsNullOrWhiteSpace(otpCode))
            throw new ArgumentException("OTP code cannot be empty", nameof(otpCode));

        try
        {
            var allOtpsForUser = await DbSet
             .Where(o => o.UserId == userId)
             .ToListAsync();

            Console.WriteLine($"Found {allOtpsForUser.Count} OTPs for user {userId}");
            foreach (var otpRecord in allOtpsForUser)
            {
                Console.WriteLine($@"OTP Record:
                Code: {otpRecord.OtpCode}
                Expiration: {otpRecord.ExpirationDate}
                IsUsed: {otpRecord.IsUsed}
                Current UTC Time: {DateTime.Now}
                Is Expired: {otpRecord.ExpirationDate <= DateTime.Now}");
            }

            var otp = await DbSet
                .Where(o =>
                    o.UserId == userId &&
                    o.OtpCode == otpCode &&
                    o.ExpirationDate > DateTime.Now &&
                    !o.IsUsed)
                .FirstOrDefaultAsync();

            if (otp == null)
            {
                Console.WriteLine($@"No valid OTP found with criteria:
                UserId: {userId}
                OtpCode: {otpCode}
                Current UTC Time: {DateTime.Now}");
                throw new ResourceNotFoundException(typeof(OtpVerification));
            }

            return otp;
        }
        catch (Exception ex) when (!(ex is ResourceNotFoundException))
        {
            throw new Exception($"Error retrieving active OTP: {ex.Message}", ex);
        }
    }

    public async Task<bool> HasActiveOtpAsync(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("Invalid user ID", nameof(userId));

        try
        {
            return await DbSet.AnyAsync(o =>
                o.UserId == userId &&
                o.ExpirationDate > DateTime.Now &&
                !o.IsUsed);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error checking active OTP: {ex.Message}", ex);
        }
    }

    public async Task DeleteExpiredOtpsAsync()
    {
        try
        {
            var expiredOtps = await DbSet
                .Where(o => o.ExpirationDate < DateTime.Now)
                .ToListAsync();

            if (expiredOtps.Any())
            {
                DbSet.RemoveRange(expiredOtps);
                await Context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting expired OTPs: {ex.Message}", ex);
        }
    }

    public async Task InvalidateUserOtpsAsync(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("Invalid user ID", nameof(userId));

        try
        {
            var activeOtps = await DbSet
                .Where(o =>
                    o.UserId == userId &&
                    o.ExpirationDate > DateTime.Now &&
                    !o.IsUsed)
                .ToListAsync();

            foreach (var otp in activeOtps)
            {
                otp.IsUsed = true;
                DbSet.Update(otp);
            }

            await Context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error invalidating user OTPs: {ex.Message}", ex);
        }
    }
}