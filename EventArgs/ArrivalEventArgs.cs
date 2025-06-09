using TestTask.Models;

namespace TestTask.EventArgs
{
    public class ArrivalEventArgs : System.EventArgs
    {
        public string FactoryName { get; private set; }
        public Product Product { get; private set; }
        public int ProductsCount { get; private set; }

        public ArrivalEventArgs(string factoryName, Product product, int productsCount)
        {
            this.FactoryName = factoryName;
            this.Product = product;
            this.ProductsCount = productsCount;
        }
    }
}
