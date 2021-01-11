using Microsoft.Data.SqlClient;
using SmartBuy.Common.Utilities.Repository;
using SmartBuy.OrderManagement.Application.InputDTOs;
using SmartBuy.OrderManagement.Application.ViewModels;
using SmartBuy.OrderManagement.Domain;
using SmartBuy.OrderManagement.Domain.Services;
using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using SmartBuy.OrderManagement.Infrastructure.Abstractions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBuy.OrderManagement.Application
{
    public class OrderApp
    {
        private readonly IManageOrderRepository _manageOrderRepository;
        private readonly IGenericReadRepository<GasStation> _gasStationRepository;
        private readonly OrderGenerator _orderGenerator;

        public OrderApp(IManageOrderRepository manageOrderRepository
            , IGenericReadRepository<GasStation> gasStationRepository
            , OrderGenerator orderGenerator)
        {
            _manageOrderRepository = manageOrderRepository;
            _gasStationRepository = gasStationRepository;
            _orderGenerator = orderGenerator;
        }

        public async Task<OrderViewModel> AddAsync(OrderInputDTO orderInput)
        {
            var inputOrder = new InputOrder
            {
                Comments = orderInput.Comments,
                CarrierId = orderInput.CarrierId,
                FromTime = orderInput.FromDateTime,
                ToTime = orderInput.ToDateTime,
                GasStationId = orderInput.GasStationId,
                OrderType = orderInput.OrderType,
                LineItems = orderInput.LineItems.Select(
                    x => new InputOrderProduct
                    {
                        Quantity = x.Quantity,
                        TankId = x.TankId
                    })
            };

            var gasStation = await _gasStationRepository
                .FindByKeyAsync(orderInput.GasStationId)
                .ConfigureAwait(false);

            var result = Order.Create(inputOrder, gasStation);

            if (result.IsSuccess)
            {
                try
                {
                    var order = await _orderGenerator.SaveAsync(result.Entity!);

                    if (!order.IsConflicting)
                    {
                        order = await _manageOrderRepository
                             .GetOrderByGasStationIdDeliveryDateAsync(order.GasStationId,
                             order.DispatchDate);

                        return new OrderViewModel
                        {
                            IsSuccess = true,
                            Message = new[] { OrderConstant.successMessage },
                            OrderId = order.Id
                        };
                    }
                    else
                    {
                        return new OrderViewModel
                        {
                            IsSuccess = false,
                            Message = new[] { OrderConstant.duplicateOrderMessage }
                        };
                    }
                }
                catch (Exception e) when (e.InnerException.GetType() == typeof(SqlException))
                {
                    return new OrderViewModel
                    {
                        IsSuccess = false,
                        Message = new[] { e.Message }
                    };
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                return new OrderViewModel
                {
                    IsSuccess = false,
                    Message = result.ErrorMessages!
                };
            }
        }
    }
}
