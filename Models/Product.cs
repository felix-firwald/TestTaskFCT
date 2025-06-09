using TestTask.Interfaces;

namespace TestTask.Models
{
    /// <summary>
    /// Класс для продукта
    /// </summary>
    public class Product : IBusinessModel
    {
        /// <summary>
        /// ID продукта (товара)
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Наименование продукта (товара)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Вес продукта (товара)
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// Тип упаковки
        /// Если типы были бы известны по ТЗ и гарантировалось, что все типы упаковки известны до этапа компиляции, я бы использовал enum
        /// </summary>
        public string PackagingType { get; set; }

        public Product(string name, double weight, string type)
        {
            this.Id = Guid.NewGuid();
            this.Name = name;
            this.Weight = weight;
            this.PackagingType = type;
        }

        public override string ToString()
        {
            return $"Продукт [{this.Id}] - {this.Name}, {this.Weight} кг., тип упаковки: {this.PackagingType}";
        }
    }
}
