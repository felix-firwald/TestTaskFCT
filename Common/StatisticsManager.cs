using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.Models;

namespace TestTask.Common
{
    public static class StatisticsManager
    {
        private static List<StatisticsTruckInfo> truckInfos = new();
        public static void AddTruckInfo(StatisticsTruckInfo truckInfo)
        {
            truckInfos.Add(truckInfo);
        }
        public static void ShowStatistics()
        {
            foreach (StatisticsTruckInfo truckInfo in truckInfos)
            {
                Globals.SendIncomingMessage(truckInfo.ToString());
            }
        }
    }
}
