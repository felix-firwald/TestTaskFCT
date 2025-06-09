using System.Collections.Concurrent;
using TestTask.EventArgs;
using TestTask.Exceptions;
using TestTask.Interfaces;

namespace TestTask.Models
{
    /// <summary>
    /// Склад для хранения продукции
    /// </summary>
    public class Warehouse : IBusinessModel
    {
        private static int capacityMultiplier = 100;

        /// <summary>
        /// Множитель вместимости
        /// </summary>
        public static int CapacityMultiplier
        {
            get => capacityMultiplier;
            set
            {
                if (capacityMultiplier != value)
                {
                    if (value < 100)
                    {
                        throw new ValueMultiplierException($"Значение множителя вместимости завода не может быть менее 100");
                    }
                    capacityMultiplier = value;
                }
            }
        }
        /// <summary>
        /// ID склада
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Наименование склада
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Объект синхронизации доступа к storedProducts
        /// </summary>
        private readonly object locker = new();

        /// <summary>
        /// Предельно допустимый порог загруженности склада (в процентах)
        /// </summary>
        private const double unloadThreshold = 0.95;

        /// <summary>
        /// Поступающие, они же отгруженные товары (продукты)
        /// </summary>
        private readonly ConcurrentQueue<(string FactoryName, Product Product)> incomingProducts = new();

        /// <summary>
        /// Хранящиеся товары (продукты)
        /// </summary>
        private readonly List<Product> storedProducts = [];

        /// <summary>
        /// Заводы
        /// </summary>
        private readonly List<Factory> factories;

        /// <summary>
        /// Грузовики
        /// </summary>
        private readonly List<Truck> trucks;

        /// <summary>
        /// Производительность в час
        /// </summary>
        private readonly double totalProductivityPerHour;

        /// <summary>
        /// Возникает при поступлении товара (продукта) на склад
        /// </summary>
        public event EventHandler<ArrivalEventArgs> OnProductArrival;

        /// <summary>
        /// Возникает при отправлении товара (продукта) со склада
        /// </summary>
        public event EventHandler<DepartureEventArgs> OnTruckDeparture;

        /// <summary>
        /// Вместимость склада
        /// </summary>
        private readonly double maxCapacity;

        public Warehouse(string name, List<Factory> factories, List<Truck> trucks)
        {
            this.Id = Guid.NewGuid();
            this.Name = name;
            this.factories = factories;
            this.trucks = trucks;
            this.totalProductivityPerHour = factories.Sum(f => f.UnitsPerHour);
            this.maxCapacity = Warehouse.CapacityMultiplier * this.totalProductivityPerHour;
        }

        /// <summary>
        /// Запуск производства
        /// </summary>
        public void StartProduction()
        {
            foreach (Factory factory in this.factories)
            {
                Task.Run(() => this.Produce(factory));
            }

            Task.Run(() => this.Unload());
        }

        /// <summary>
        /// Производство
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        private async Task Produce(Factory factory)
        {
            while (true)
            {
                Product product = factory.ProductGenerator();
                incomingProducts.Enqueue((factory.Name, product));
                this.DoProductArrival(factory.Name, product);
                TimeSpan ts = TimeSpan.FromSeconds(3600.0 / factory.UnitsPerHour);
                await Task.Delay(ts);
            }
        }

        private async Task Unload()
        {
            while (true)
            {
                double currentLoad = 0;

                lock (locker)
                {
                    currentLoad = storedProducts.Count;
                }

                if (currentLoad >= maxCapacity * unloadThreshold)
                {
                    Console.WriteLine("Начата разгрузка...");
                    await this.DispatchTruck();
                }
                else
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
            }
        }

        private async Task DispatchTruck()
        {
            Truck truck = this.GetAvailableTruck();

            if (truck != null)
            {
                this.LoadTruck(truck);
                this.DoTruckDeparture(truck);
                await Task.Delay(TimeSpan.FromSeconds(10));
            }
            else
            {
                Console.WriteLine("Нет свободных грузовиков. Ожидание...");
                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }

        private Truck GetAvailableTruck()
        {
            return trucks.FirstOrDefault();
        }

        private void LoadTruck(Truck truck)
        {
            lock (locker)
            {
                truck.ClearProducts();
                int productsToLoad = (int)Math.Min(truck.Capacity, storedProducts.Count);

                for (int i = 0; i < productsToLoad; i++)
                {
                    if (storedProducts.Count > 0)
                    {
                        truck.AddProduct(storedProducts[0]);
                        storedProducts.RemoveAt(0);
                    }
                }
                Console.WriteLine($"Грузовик {truck.Id} загружен, кол-во продуктов: {truck.CurrentProductsCount}. Текущая загрузка: {truck.CurrentLoadWeight}");
            }
        }

        protected virtual void DoProductArrival(string factoryName, Product product)
        {
            this.OnProductArrival?.Invoke(this, new ArrivalEventArgs(factoryName, product, storedProducts.Count+1));

            lock (locker)
            {
                storedProducts.Add(product);
            }
        }

        protected virtual void DoTruckDeparture(Truck truck)
        {
            this.OnTruckDeparture?.Invoke(this, new DepartureEventArgs(truck));
        }

        public void ProcessIncomingProducts()
        {
            while (incomingProducts.TryDequeue(out (string FactoryName, Product Product) item))
            {
                string factoryName = item.FactoryName;
                Product product = item.Product;

                lock (locker)
                {
                    storedProducts.Add(product);
                }
                this.DoProductArrival(factoryName, product);
            }
        }
    }
}
