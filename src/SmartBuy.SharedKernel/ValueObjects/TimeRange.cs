using System;

namespace SmartBuy.SharedKernel.ValueObjects
{
    public class TimeRange : ValueObject<TimeRange>
    {
        public TimeSpan Start { get; private set; }
        public TimeSpan End { get; private set; }

        public TimeRange(TimeSpan start, TimeSpan end)
        {
            Start = start;
            End = end;
        }

        protected TimeRange() { }

        public int DurationInMinutes()
        {
            return (End - Start).Minutes;
        }

        public TimeRange NewEnd(TimeSpan newEnd)
        {
            return new TimeRange(this.Start, newEnd);
        }

        public TimeRange NewStart(TimeSpan newStart)
        {
            return new TimeRange(newStart, this.End);
        }

        public bool Overlaps(TimeRange dateTimeRange)
        {
            return this.Start < dateTimeRange.End &&
                this.End > dateTimeRange.Start;
        }
    }
}
