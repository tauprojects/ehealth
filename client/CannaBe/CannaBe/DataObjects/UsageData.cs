using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CannaBe
{
    class UsageData
    {
        private readonly UserData User;
        private readonly Strain UsageStrain;
        private readonly DateTime StartTime;

        public UsageData(UserData user, Strain usageStrain, DateTime startTime)
        {
            User = user;
            UsageStrain = usageStrain;
            StartTime = startTime;
        }

        private long HeartRateReadings { get; set; } = 0;

        public double HeartRateAverage { get; private set; } = 0;
        public int HeartRateMin { get; private set; } = int.MaxValue;
        public int HeartRateMax { get; private set; } = 0;

        public void HeartRateChanged(int rate, double accuracy)
        {
            if (accuracy == 1)
            {
                HeartRateReadings++;

                //from: math.stackexchange.com/questions/106700/incremental-averageing
                HeartRateAverage += (rate - HeartRateAverage) / HeartRateReadings;

                HeartRateMin = Math.Min(HeartRateMin, rate);
                HeartRateMax = Math.Max(HeartRateMax, rate);

                if(HeartRateReadings % 10 == 0)
                {
                    AppDebug.Line($"HeartRate: cur<{rate}> min<{HeartRateMin}> avg<{Math.Round(HeartRateAverage, 2)}> max<{HeartRateMax}>");
                }
            }
        }
    }
}
