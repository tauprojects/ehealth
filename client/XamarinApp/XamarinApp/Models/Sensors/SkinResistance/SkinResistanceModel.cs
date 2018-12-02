using System;
using System.Runtime.Serialization;

namespace Band
{
    [DataContract]
    public class SkinResSensorReading : ViewModel
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

        double _skinRes;

        [DataMember]
        public double SkinRes
        {
            get { return _skinRes; }
            set
            {
                SetValue(ref _skinRes, value, "SkinRes");
            }
        }

        public double Value
        {
            get
            {
                return SkinRes;
            }
        }
    }
}
