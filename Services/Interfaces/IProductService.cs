using Assignment2.Models;

namespace Assignment2.Services.Interfaces
{
    public interface IProductService
    {

        public List<Product> GetAll();

        public Product GetById(int id);

        public void Add(Product product);

        public void Update(Product product);

        public void Delete(int id);
    };
}
