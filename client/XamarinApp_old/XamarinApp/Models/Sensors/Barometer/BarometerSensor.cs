using System;
using System.Runtime.Serialization;

namespace Band
{
    [DataContract]
    public class BarometerSensorReading : ViewModel
    {
        DateTimeOffset _timestamp;

        [DataMember]
        public DateTimeOffset Timestamp
        {
            get { return _timestamp; }
            set
            {
                SetValue(ref _timestamp, value, "Timestamp");
            }
        }

        double _temperature;

        [DataMember]
        public double Temperature
        {
            get { return _temperature; }
            set
            {
                SetValue(ref _temperature, value, "Temperature");
            }
        }

        public double Value
        {
            get
            {
                return Temperature;
            }
        }
    }
}
