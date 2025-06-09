using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.Interfaces;

namespace TestTask.Models
{
    /// <summary>
    /// Класс для завода, производящего продукцию - см. класс <seealso cref="Models.Product"/>
    /// </summary>
    public class Factory : IBusinessModel
    {
        /// <summary>
        /// Factory id
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Name of factory
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The number of units a factory produces per hour
        /// </summary>
        public double UnitsPerHour { get; set; }

        /// <summary>
        /// Type of products that the factory specializes in
        /// </summary>
        public string ProductType { get; set; }

        public Func<Product> ProductGenerator { get; set; }

        public Factory(string name, string productType, double units, Func<Product> productGenerator)
        {
            this.Id = Guid.NewGuid();
            this.Name = name;
            this.ProductType = productType;
            this.UnitsPerHour = units;
            this.ProductGenerator = productGenerator;
        }
    }
}
