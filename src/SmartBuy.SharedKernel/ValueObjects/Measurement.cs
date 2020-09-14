using SmartBuy.SharedKernel.Enums;

namespace SmartBuy.SharedKernel.ValueObjects
{
    public class Measurement : ValueObject<Measurement>
    {
        public int NetQuantity { get; private set; }

        public TankMeasurement Unit { get; private set; }

        public int Top { get; private set; }

        public int Bottom { get; private set; }

        public int Quantity { get; private set; }

        public Measurement()
        {

        }

        public Measurement(int netQuantity, TankMeasurement unit,
            int top, int bottom, int quantity)
        {
            NetQuantity = netQuantity;
            Unit = unit;
            Top = top;
            Bottom = bottom;
            Quantity = quantity;
        }

        public Measurement ChangeUnit(TankMeasurement unit)
        {
            return new Measurement(this.NetQuantity, unit, this.Top, this.Bottom, this.Quantity);
        }

        public Measurement UpdateNetQuantity(int netQuantity)
        {
            return new Measurement(netQuantity, this.Unit, this.Top, this.Bottom, this.Quantity);
        }

        public Measurement UpdateQuantity(int quantity)
        {
            return new Measurement(this.NetQuantity, this.Unit, this.Top, this.Bottom, quantity);
        }

        public Measurement UpdateTankTop(int top)
        {
            return new Measurement(this.NetQuantity, this.Unit, top, this.Bottom, this.Quantity);
        }

        public Measurement UpdateTankBottom(int bottom)
        {
            return new Measurement(this.NetQuantity, this.Unit, this.Top, bottom, this.Quantity);
        }
    }
}
