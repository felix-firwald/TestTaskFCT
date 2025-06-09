using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.Exceptions;
using TestTask.Models;

namespace TestTask.Common
{
    public static class Globals
    {
        public delegate void IncomingMessage(string message);
        public static event IncomingMessage OnIncomingMessage;
        #region Properties
        private static int unitsPerHourMultiplier = 50;
        public static int UnitsPerHourMultiplier
        {
            get
            {
                return unitsPerHourMultiplier;
            }
            set
            {
                if (unitsPerHourMultiplier != value)
                {
                    if (value < 50)
                    {
                        throw new ValueMultiplierException($"Значение множителя единиц продукции завода не может быть менее 50");
                    }
                    unitsPerHourMultiplier = value;
                    OnUnitsPerHourMultiplierChanged?.Invoke(value);
                }
            }
        }
        public static List<Factory> Factories { get; set; }
        public static List<Truck> Trucks { get; set; }
        #endregion

        #region Methods
        public static void DoMainWork()
        {
            
            UnitsPerHourMultiplier = 50000;
            Factories = new List<Factory>
            {
                new("Завод 1 (A)", "ProductA", UnitsPerHourMultiplier, () => new Product("ProductA", 1.0, "Картонная коробка")),
                new("Завод 2 (B)", "ProductB", 1.1 * UnitsPerHourMultiplier, () => new Product("ProductB", 1.2, "Пластиковая коробка")),
                new("Завод 3 (C)", "ProductC", 1.2 * UnitsPerHourMultiplier, () => new Product("ProductC", 0.8, "Металлическая коробка"))
            };
            Trucks = new List<Truck>
            {
                new("Грузовик 1", 500),
                new("Грузовик 2", 600)
            };
            Warehouse warehouse = new("Главный склад", Factories, Trucks);
            warehouse.OnProductArrival += (sender, e) =>
            {
                SendIncomingMessage($"Поступление продукта с завода {e.FactoryName}: {e.Product}. Количество продукции: {e.ProductsCount}");
            };
            warehouse.OnTruckDeparture += (sender, e) =>
            {
                SendIncomingMessage($"Грузовик {e.Truck.Id} уехал, количество продуктов: {e.Truck.CurrentProductsCount}");
            };
            warehouse.StartProduction();
            Thread.Sleep(30000);
            StatisticsManager.ShowStatistics();

        }
        public static void SendIncomingMessage(string message)
        {
            OnIncomingMessage?.Invoke(message);
        }
        #endregion

        #region Events & delegates
        public delegate void MultiplierChangedHandler(int newValue);
        public static event MultiplierChangedHandler OnUnitsPerHourMultiplierChanged;
        #endregion
    }
}