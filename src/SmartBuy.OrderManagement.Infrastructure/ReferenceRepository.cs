using SmartBuy.Common.Utilities.Repository;
using SmartBuy.OrderManagement.Infrastructure.Abstractions;

namespace SmartBuy.OrderManagement.Infrastructure
{ 
    public sealed class ReferenceRepository<TEntity> : 
        GenericReadRepository<TEntity>, IReferenceRepository<TEntity>
        where TEntity : class
    {
        public ReferenceRepository(ReferenceContext context)
            : base(context)
        {

        }
    }
}
