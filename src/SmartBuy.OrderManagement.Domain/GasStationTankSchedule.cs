namespace SmartBuy.OrderManagement.Domain
{
    public class GasStationTankSchedule
    {
        private GasStationTankSchedule()
        {

        }

        public GasStationTankSchedule(int tankId, int quantity)
        {
            TankId = tankId;
            Quantity = quantity;
        }

        public int TankId { get; private set; }

        public int Quantity { get; private set; }
    }
}
