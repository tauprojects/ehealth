using System;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace CannaBe
{
    class UsageData
    {
        public delegate void ChangeHandler(double avg, int min, int max);
        private readonly UserData User;
        public readonly Strain UsageStrain;
        public readonly DateTime StartTime;
        // private readonly DateTime EndTime;

        public ChangeHandler Handler = null;

        public bool UseBandData { get; set; } = false;

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

        public void HeartRateChangedAsync(int rate, double accuracy)
        {
            if (accuracy == 1)
            {
                HeartRateReadings++;

                //from: math.stackexchange.com/questions/106700/incremental-averageing
                HeartRateAverage += (rate - HeartRateAverage) / HeartRateReadings;

                HeartRateMin = Math.Min(HeartRateMin, rate);
                HeartRateMax = Math.Max(HeartRateMax, rate);

                if (HeartRateReadings % 5 == 0)
                {
                    AppDebug.Line($"HeartRate: cur<{rate}> min<{HeartRateMin}> avg<{Math.Round(HeartRateAverage, 0)}> max<{HeartRateMax}>");
                    try
                    {
                        CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            Handler?.Invoke(HeartRateAverage, HeartRateMin, HeartRateMax);
                        }).AsTask().GetAwaiter().GetResult();

                    }
                    catch (Exception e)
                    {
                        AppDebug.Exception(e, "HeartRateChanged");
                    }
                }
            }
        }
    }
}
