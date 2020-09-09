using System; 

namespace SmartBuy.OrderManagement.Domain.Services.Abstractions
{
    public interface ITimeIntervalComparable
    {
        bool Compare(TimeSpan interval, DateTime recentDeliveryDate, DateTime targetDate);
    }
}
