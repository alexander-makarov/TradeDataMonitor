using System;
using System.Linq;

namespace TradeDataMonitoring
{
    public class TradeData
    {
        public DateTime Date { get; private set; }
        public Decimal Open { get; private set; }
        public Decimal High { get; private set; }
        public Decimal Low { get; private set; }
        public Decimal Close { get; private set; }
        public int Volume { get; private set; }

        public TradeData(DateTime date, Decimal open, Decimal high, Decimal low, Decimal close, int volume)
        {
            Date = date;
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
        }

        public static TradeData Parse(string[] valuesArray)
        {
            if (valuesArray == null)
            {
                throw new ArgumentNullException("valuesArray");
            }
            if (valuesArray.Length < 6)
            {
                throw new FormatException("Expected 6 values in array to parse");
            }
            if (valuesArray.Any(String.IsNullOrWhiteSpace))
            {
                throw new FormatException("Can't parse some fo the values because they are null, empty strings, or whitespaces only");
            }
            var data = new TradeData(
                DateTime.Parse(valuesArray[0]),
                Decimal.Parse(valuesArray[1]),
                Decimal.Parse(valuesArray[2]),
                Decimal.Parse(valuesArray[3]),
                Decimal.Parse(valuesArray[4]),
                Int32.Parse(valuesArray[5]));

            return data;
        }
    }
}