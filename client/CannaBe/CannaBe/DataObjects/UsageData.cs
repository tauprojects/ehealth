using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace CannaBe
{
    class UsageData : ViewModel
    {
        public delegate void HeartRateUpdateHandler(double avg, int min, int max);
        public Strain UsageStrain { get; private set; }
        private DateTime startTime;
        public Dictionary<string,string> usageFeedback;
        public DateTime StartTime
        {
            get
            {
                return startTime;
            }
            private set
            {
                startTime = value;
                OnPropertyChanged("StartTime");
            }
        }
        public string StartTimeString
        {
            get
            {
                return StartTime.ToString("MMMM dd, yyyy HH:mm:ss");
            }
        }

        public DateTime EndTime { get; private set; }

        private TimeSpan duration;
        public TimeSpan Duration
        {
            get
            {
                return duration;
            }
            private set
            {
                duration = value;
                OnPropertyChanged("Duration");
            }
        }
        public string DurationString
        {
            get
            {
                StringBuilder str = new StringBuilder("");
                if (Duration.Hours > 0)
                {
                    str.Append($"{Duration.Hours} hours, ");
                }
                if (Duration.Minutes > 0)
                {
                    str.Append($"{Duration.Minutes} mins, ");
                }
                if (Duration.TotalSeconds >= 1)
                {
                    str.Append($"{Duration.Seconds} secs");
                }
                else
                {
                    str.Append($"{Duration.TotalMilliseconds} ms");
                }

                return str.ToString();
            }
        }

        //DispatcherTimer Timer = new DispatcherTimer();

        public HeartRateUpdateHandler Handler = null;

        private bool usedBandData = false;
        public bool UseBandData
        {
            get
            {
                return usedBandData;
            }
            set
            {
                usedBandData = value;
                OnPropertyChanged("UseBandData");
            }
        }
        
        public string UsedBandString
        {
            get
            {
                return UseBandData ? "Yes" : "No";
            }
        }

        public UsageData(Strain usageStrain, DateTime startTime, bool _useBandData)
        {
            UsageStrain = usageStrain;
            StartTime = startTime;
            UseBandData = _useBandData;
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
            Duration = EndTime.Subtract(StartTime);
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
