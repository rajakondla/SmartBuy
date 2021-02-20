using SmartBuy.Common.Utilities.Repository;
using System;

namespace SmartBuy.Administration.Infrastructure.Abstraction
{
    public interface IAdministrationRepository<TEntity> :
         IGenericReadRepository<TEntity> where TEntity : class
    {

    }
}
