using Microsoft.Data.SqlClient;
using SmartBuy.Common.Utilities.Repository;
using SmartBuy.OrderManagement.Application.InputDTOs;
using SmartBuy.OrderManagement.Application.ViewModels;
using SmartBuy.OrderManagement.Domain;
using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using SmartBuy.OrderManagement.Infrastructure.Abstractions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBuy.OrderManagement.Application
{
    public class OrderAPI
    {
        private readonly IManageOrderRepository _manageOrderRepository;
        private readonly IGenericReadRepository<GasStation> _gasStationRepository;

        public OrderAPI(IManageOrderRepository manageOrderRepository
            , IGenericReadRepository<GasStation> gasStationRepository)
        {
            _manageOrderRepository = manageOrderRepository;
            _gasStationRepository = gasStationRepository;
        }

        public async Task<OrderViewModel> Add(OrderInputDTO orderInput)
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
                var manageOrder = new ManageOrder();
                manageOrder.Add(result.Entity!);

                try
                {
                    await _manageOrderRepository.UpsertAsync(manageOrder);
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

                var orderId = (await _manageOrderRepository.GetOrdersByGasStationIdAsync(orderInput.GasStationId,
                    orderInput.OrderType)).Orders.First().Id;

                return new OrderViewModel { OrderId = orderId, IsSuccess = true };
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
