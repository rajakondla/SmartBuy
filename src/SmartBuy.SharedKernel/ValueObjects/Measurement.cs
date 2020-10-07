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

        public Measurement(TankMeasurement unit,
            int top, int bottom, int quantity)
        {
            Unit = unit;
            Top = top;
            Bottom = bottom;
            Quantity = quantity;
            NetQuantity = top + bottom + quantity;
        }

        public Measurement ChangeUnit(TankMeasurement unit)
        {
            return new Measurement(unit, this.Top, this.Bottom, this.Quantity);
        }

        public Measurement UpdateNetQuantity(int netQuantity)
        {
            return new Measurement(this.Unit, this.Top, this.Bottom, this.Quantity);
        }

        public Measurement UpdateQuantity(int quantity)
        {
            return new Measurement(this.Unit, this.Top, this.Bottom, quantity);
        }

        public Measurement UpdateTankTop(int top)
        {
            return new Measurement(this.Unit, top, this.Bottom, this.Quantity);
        }

        public Measurement UpdateTankBottom(int bottom)
        {
            return new Measurement(this.Unit, this.Top, bottom, this.Quantity);
        }
    }
}
