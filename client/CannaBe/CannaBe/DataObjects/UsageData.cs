using System;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace CannaBe
{
    class UsageData
    {
        public delegate void HeartRateUpdateHandler(double avg, int min, int max);
        public Strain UsageStrain { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; set; }

        public HeartRateUpdateHandler Handler = null;

        public bool UseBandData { get; set; } = false;

        public UsageData(Strain usageStrain, DateTime startTime)
        {
            UsageStrain = usageStrain;
            StartTime = startTime;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is UsageData other))
            {
                return false;
            }

            return StartTime.Equals(other.StartTime) && EndTime.Equals(other.EndTime);
        }

        public override int GetHashCode()
        {
            return StartTime.GetHashCode();
        }

        public void EndUsage()
        {
            EndTime = DateTime.Now;
            GlobalContext.CurrentUser.UsageSessions.Add(this);
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
                        //Run code on main thread for UI change, preventing exception
                        CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            try
                            {
                                Handler?.Invoke(HeartRateAverage, HeartRateMin, HeartRateMax);
                            }
                            catch (Exception x)
                            {
                                AppDebug.Exception(x, "HeartRateChangedAsync => Handler?.Invoke");
                            }
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
