using SmartBuy.OrderManagement.Infrastructure.Abstractions.DTOs;

namespace SmartBuy.OrderManagement.Domain.Services
{
    internal class OrderCreationFactory
    {
        public IOrderCreation Generate(GasStationDetailDTO gasStationDetail)
        {
            //switch (gasStationDetail.OrderType)
            //{
            //    case OrderType.Schedule:
            //        return new ScheduleOrder(gasStationDetail);
            //    case OrderType.Estimate:
            //        return new EstimateOrder(gasStationDetail);
            //    case OrderType.Historical:
            //        return new HistoricalOrder(gasStationDetail);
            //}
            return null;
        }
    }
}
