using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.Interfaces;

namespace TestTask.Models
{
    /// <summary>
    /// Класс грузовика
    /// </summary>
    public class Truck : IBusinessModel
    {
        /// <summary>
        /// ID грузовика
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Наименование грузовика
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Вместимость грузовика
        /// </summary>
        public double Capacity { get; private set; }

        /// <summary>
        /// Товары (продукты) - см. класс <seealso cref="Models.Product"/>, которые в текущий момент транспортируются грузовиком.
        /// При создании объекта грузовика товаров ещё нет
        /// </summary>
        private List<Product> TransportedProducts { get; set; } = new();

        /// <summary>
        /// Текущая загруженность, выражена в весе товаров (продуктов)
        /// </summary>
        public double CurrentLoadWeight
        {
            get
            {
                return this.TransportedProducts.Sum(p => p.Weight);
            }
        }

        /// <summary>
        /// Количество товаров (продуктов)
        /// </summary>
        public double CurrentProductsCount
        {
            get
            {
                return this.TransportedProducts.Count;
            }
        }

        /// <summary>
        /// Полностью ли загружен грузовик.
        /// Если нет: можно продолжать загружать товары
        /// Если да: нельзя
        /// </summary>
        public bool IsCapacityIsDone
        {
            get
            {
                return this.CurrentLoadWeight >= this.Capacity;
            }
        }

        public Truck(string name, double capacity)
        {
            this.Id = Guid.NewGuid();
            this.Name = name;
            this.Capacity = capacity;
        }

        public void ClearProducts()
        {
            this.TransportedProducts.Clear();
        }
        public void AddProduct(Product product)
        {
            this.TransportedProducts.Add(product);
        }
    }
}
