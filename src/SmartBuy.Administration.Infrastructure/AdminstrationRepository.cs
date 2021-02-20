using SmartBuy.Administration.Infrastructure.Abstraction;
using SmartBuy.Common.Utilities.Repository;

namespace SmartBuy.Administration.Infrastructure
{
    public sealed class AdminstrationRepository<TEntity> 
        : GenericReadRepository<TEntity>, IAdministrationRepository<TEntity>
        where TEntity : class
    {
        public AdminstrationRepository(AdministrationContext context)
            : base(context)
        {

        }
    }
}
