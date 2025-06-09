using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.Models;

namespace TestTask.Common
{
    public struct StatisticsTruckInfo
    {
        public readonly Truck TargetTruck { get; }
        public readonly double ProductsCount { get; }
        public readonly double AverageWeight { get; }
        public StatisticsTruckInfo(Truck truck, double products, double averageWeight)
        {
            this.TargetTruck = truck;
            this.ProductsCount = products;
            this.AverageWeight = averageWeight;
        }
        public void Register()
        {
            StatisticsManager.AddTruckInfo(this);
        }
        public override string ToString()
        {
            return $"{this.TargetTruck}, кол-во перевезенных продуктов: {this.ProductsCount}, средний вес продукта: {this.AverageWeight}";
        }
    }
}
