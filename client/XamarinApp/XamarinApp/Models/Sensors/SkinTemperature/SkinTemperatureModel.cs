using System;
using System.Runtime.Serialization;

namespace Band
{
    [DataContract]
    public class SkinTempSensorReading : ViewModel
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

        double _skinTemp;

        [DataMember]
        public double SkinTemp
        {
            get { return _skinTemp; }
            set
            {
                SetValue(ref _skinTemp, value, "SkinTemp");
            }
        }

        public double Value
        {
            get
            {
                return SkinTemp;
            }
        }
    }
}
