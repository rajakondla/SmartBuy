using SmartBuy.SharedKernel;

namespace SmartBuy.OrderManagement.Domain.Services
{
    public class DefaultOutputDomainResult
    {
        private static OutputDomainResult<Order> _order = new OutputDomainResult<Order>(false);
        private static DefaultOutputDomainResult _instance = new DefaultOutputDomainResult();

        private DefaultOutputDomainResult()
        {

        }

        public static DefaultOutputDomainResult GetInstance => _instance;

        public OutputDomainResult<Order> Order => _order;
    }
}
