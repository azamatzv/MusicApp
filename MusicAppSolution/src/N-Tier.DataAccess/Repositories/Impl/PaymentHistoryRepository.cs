using Microsoft.EntityFrameworkCore;
using N_Tier.Core.Entities;
using N_Tier.DataAccess.Persistence;
using System.Linq.Expressions;

namespace N_Tier.DataAccess.Repositories.Impl;

public class PaymentHistoryRepository : BaseRepository<PaymentHistory>, IPaymentHistoryRepository
{
    private readonly DatabaseContext context;

    public PaymentHistoryRepository(DatabaseContext context) : base(context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<PaymentHistory>> GetAllHistoryAsync(
    Expression<Func<PaymentHistory, bool>> filter,
    string includeProperties = "")
    {
        IQueryable<PaymentHistory> query = context.PaymentHistories;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        return await query.ToListAsync();
    }

    public async Task<PaymentHistory> GetFirstHistoryAsync(Expression<Func<PaymentHistory, bool>> predicate)
    {
        return await context.PaymentHistories
        .FirstOrDefaultAsync(predicate);
    }

}
