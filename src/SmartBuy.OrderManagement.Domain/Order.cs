﻿using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using SmartBuy.OrderManagement.Domain.Validattion;
using SmartBuy.SharedKernel;
using SmartBuy.SharedKernel.Enums;
using SmartBuy.SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartBuy.OrderManagement.Domain
{
    public class Order : Entity<int>
    {
        public Guid GasStationId { get; private set; }

        public string? Comments { get; private set; }

        public DateTimeRange DispatchDate { get; private set; }

        public ICollection<OrderProduct> OrderProducts { get; private set; }

        public OrderType OrderType { get; private set; }

        public Guid? CarrierId { get; private set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        #region non persisted
        public TrackingState State { get; set; }

        public bool IsConflicting { get; set; }

        #endregion

        private Order()
        {
            OrderProducts = new List<OrderProduct>();
            DispatchDate = new DateTimeRange(DateTime.Now, DateTime.Now);
        }

        public static OutputDomainResult<Order> Create(InputOrder inputOrder
            , GasStation gasStation)
        {
            if (inputOrder is null)
                throw new ArgumentNullException(nameof(inputOrder));

            if (gasStation is null)
                throw new ArgumentNullException(nameof(gasStation));

            var orderValidator = new InputOrderValidator(gasStation);
            var result = orderValidator.Validate(inputOrder);
            if (result.IsValid)
            {
                var order = new Order
                {
                    GasStationId = inputOrder.GasStationId,
                    Comments = inputOrder.Comments,
                    OrderType = inputOrder.OrderType,
                    DispatchDate = new DateTimeRange(inputOrder.FromTime, inputOrder.ToTime),
                    CarrierId = inputOrder.CarrierId
                };
                foreach (var lineItem in inputOrder.LineItems)
                    order.InsertOrderProduct(lineItem.TankId, lineItem.Quantity);
                return new OutputDomainResult<Order>(result.IsValid, entity: order);
            }
            else
            {
                return new OutputDomainResult<Order>(result.IsValid, result.Errors.Select(x => x.ErrorMessage).ToList());
            }
        }

        private void InsertOrderProduct(int tankId, int quantity)
        {
            OrderProducts.Add(OrderProduct.Create(tankId, quantity));
        }
    }
}

