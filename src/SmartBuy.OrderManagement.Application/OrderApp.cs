using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
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
        private readonly IReferenceRepository<GasStation> _gasStationRepository;
        private readonly OrderGenerator _orderGenerator;
        private readonly ILogger<OrderApp> _logger;

        public OrderApp(IManageOrderRepository manageOrderRepository
            , IReferenceRepository<GasStation> gasStationRepository
            , OrderGenerator orderGenerator
            , ILogger<OrderApp> logger)
        {
            _manageOrderRepository = manageOrderRepository;
            _gasStationRepository = gasStationRepository;
            _orderGenerator = orderGenerator;
            _logger = logger;
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

            try
            {
                var gasStation = await _gasStationRepository
                    .FindByKeyAsync(orderInput.GasStationId)
                    .ConfigureAwait(false);

                if (gasStation != null)
                {
                    var result = Order.Create(inputOrder, gasStation);

                    if (result.IsSuccess)
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
                        //catch (Exception e) when (e.InnerException.GetType() == typeof(SqlException))
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
                else
                {
                    _logger.LogWarning(OrderConstant.GasStationIdNotFound(orderInput.GasStationId));
                    return new OrderViewModel
                    {
                        IsSuccess = false,
                        Message = new[] { OrderConstant.GasStationIdNotFound(orderInput.GasStationId) }
                    };
                }
            }
            catch (SqlException e)
            {
                _logger.LogError("Sql exception when saving order", e);
                return new OrderViewModel
                {
                    IsSuccess = false,
                    Message = new[] { "Network failure in processing order" }
                };
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError("Required parameter is not passed", e);
                return new OrderViewModel
                {
                    IsSuccess = false,
                    Message = new[] { "Internal application error" }
                };
            }
        }
    }
}
