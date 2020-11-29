using Microsoft.EntityFrameworkCore;

namespace SmartBuy.OrderManagement.Infrastructure
{
    public class BaseReferenceContext
    {
        public BaseReferenceContext(ReferenceContext context)
        {
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ReferenceContext = context;
        }

        protected ReferenceContext ReferenceContext { get; }
    }
}
