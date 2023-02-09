using Microsoft.AspNetCore.Mvc;
using System.IO.MemoryMappedFiles;
using WebAPISample.Modules;

namespace WebAPISample.Modules
{
    public class Utilization
    {
        public DateTime currentDate { get; set; } = DateTime.MinValue;
        public TimeSpan timeOfOperation { get; set; } = TimeSpan.Zero;
        public TimeSpan timeOfStopSum { get; set; } = TimeSpan.Zero;

        public TimeSpan timeOfSupplyPause { get; set; } = TimeSpan.Zero;
        public TimeSpan timeOfPause { get; set; } = TimeSpan.Zero;

        public Utilization() { }

        public Utilization(DateTime currentDate, List<Tuple<String, DateTime>> stateChangeDatas)
        {
            DateTime startTime = DateTime.MinValue;
            DateTime pauseTime = DateTime.MinValue;
            String lastStopCause = "";
            this.currentDate = currentDate;
            foreach (var changeData in stateChangeDatas)
            {
                if (changeData.Item1.Equals(UtilizationControll.STATECODE_START))
                {
                    startTime = changeData.Item2;
                }
                else if (changeData.Item1.Equals(UtilizationControll.STATECODE_END))
                {
                    if (startTime.Equals(DateTime.MinValue))
                    {
                        Console.WriteLine("ロボットのストップ前にスタートがかかった");
                        return;
                    }
                    timeOfOperation += (changeData.Item2 - startTime);
                    startTime = DateTime.MinValue;
                }
                else if (changeData.Item1.Equals(UtilizationControll.STATECODE_PAUSE))
                {
                    lastStopCause = UtilizationControll.STATECODE_PAUSE;
                    pauseTime = changeData.Item2;
                }
                else if (changeData.Item1.Equals(UtilizationControll.STATECODE_STANDBY))
                {
                    pauseTime = changeData.Item2;
                    lastStopCause = UtilizationControll.STATECODE_STANDBY;
                }
                else if (changeData.Item1.Equals(UtilizationControll.STATECODE_RESTART))
                {
                    if (startTime.Equals(DateTime.MinValue))
                    {
                        Console.WriteLine("ロボットのストップ前にリスタートがかかった");
                        return;
                    }
                    if (lastStopCause.Equals(UtilizationControll.STATECODE_PAUSE))
                    {
                        timeOfSupplyPause += (changeData.Item2 - pauseTime);
                    }
                    else if (lastStopCause.Equals(UtilizationControll.STATECODE_STANDBY))
                    {
                        timeOfPause += (changeData.Item2 - pauseTime);
                    }
                    else
                    {
                        Console.WriteLine("ロボットのストップ前にリスタートがかかった");
                    }
                    pauseTime = DateTime.MinValue;
                }
                else if (changeData.Item1.Equals(UtilizationControll.STATECODE_EMERGENCY))
                {
                    startTime = DateTime.MinValue;
                    pauseTime = DateTime.MinValue;
                }
            }
        }

    }
}