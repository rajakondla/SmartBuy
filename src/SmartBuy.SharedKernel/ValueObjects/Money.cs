using SmartBuy.SharedKernel.Enums;

namespace SmartBuy.SharedKernel.ValueObjects
{
    public class Money : ValueObject<Money>
    {
        public Money(decimal value, CurrencyUnit unit)
        {
            Value = value;
            Unit = unit;
        }

        public decimal Value { get; set; }

        public CurrencyUnit Unit { get; set; }

        public Money ChangeUnit(CurrencyUnit unit)
        {
            return new Money(this.Value, unit);
        }

        public Money UpdateValuue(decimal value)
        {
            return new Money(value, this.Unit);
        }
    }
}
