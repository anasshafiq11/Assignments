using Assignment2.Models;
using Assignment2.Services.Interfaces;


namespace Assignment2.Services
{
    public class ProductService : IProductService
    {
        private static List<Product> products = new()
        {
            new Product
            {
                Id = 1,
                Name = "Laptop",
                Price = 50000,
                Quantity = 5
            },
            new Product
            {
                Id = 2,
                Name = "Smartphone",
                Price = 20000,
                Quantity = 10
            },
             new Product
            {
                Id = 3,
                Name = "Headphones",
                Price = 5000,
                Quantity = 15
            }

        };

        public List<Product> GetAll()
        {
            return products;
        }

        public Product GetById(int id)
        {
            return products.FirstOrDefault(x => x.Id == id);
        }

        public void Add(Product product)
        {
            product.Id = products.Count + 1;
            products.Add(product);
        }

        public void Update(Product product)
        {
            var existing = GetById(product.Id);

            if (existing != null)
            {
                existing.Name = product.Name;
                existing.Price = product.Price;
                existing.Quantity = product.Quantity;
            }
        }

        public void Delete(int id)
        {
            var product = GetById(id);

            if (product != null)
                products.Remove(product);
        }
    }
}