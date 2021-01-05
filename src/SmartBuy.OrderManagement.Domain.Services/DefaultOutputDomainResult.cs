using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using SmartBuy.OrderManagement.Infrastructure.Abstractions.DTOs;
using SmartBuy.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
