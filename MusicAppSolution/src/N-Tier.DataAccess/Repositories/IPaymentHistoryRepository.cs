using N_Tier.Core.Entities;
using System.Linq.Expressions;

namespace N_Tier.DataAccess.Repositories;

public interface IPaymentHistoryRepository : IBaseRepository<PaymentHistory>
{
    Task<IEnumerable<PaymentHistory>> GetAllHistoryAsync(
    Expression<Func<PaymentHistory, bool>> filter,
    string includeProperties = "");

    Task<PaymentHistory> GetFirstHistoryAsync(Expression<Func<PaymentHistory, bool>> predicate);
}
