using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.Models;

namespace TestTask.Common
{
    public class StatisticsManager
    {
        private List<Truck> trucks = new();
        public void RegisterTruck(Truck truck)
        {
            trucks.Add(truck);
        }

        public void ShowStatistics()
        {
            if (trucks.Count == 0)
            {
                Console.WriteLine("Грузовиков нет");
                return;
            }
            Console.WriteLine("Статистика по грузовикам:");
            Console.WriteLine($"Общее количество отправленных грузовиков: {trucks.Count}");
            double averageWeight = trucks.Average(t => t.CurrentLoadWeight);
            Console.WriteLine($"Средний вес: {averageWeight}");
        }
    }
}
