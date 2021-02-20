using SmartBuy.Common.Utilities.Repository;

namespace SmartBuy.OrderManagement.Infrastructure.Abstractions
{
    public interface IReferenceRepository<TEntity> :
    IGenericReadRepository<TEntity> where TEntity : class
    {

    }
}
