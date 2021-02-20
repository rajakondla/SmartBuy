using System;

namespace SmartBuy.OrderManagement.Domain.Services.Abstractions
{
    public static class OrderConstant
    {
        public const string successMessage = "Order created successfully!";

        public const string duplicateOrderMessage = "Order already exists with same dispatch date";

        public static string GasStationIdNotFound(Guid gasStationId)
        {
           return $"Gas Station Id not found {gasStationId}";
        }
    }
}
