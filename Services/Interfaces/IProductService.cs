using Assignment2.Models;

namespace Assignment2.Services.Interfaces
{
    public interface IProductService
    {

        Task<List<Product>> GetAllAsync();

        Task<Product?> GetByIdAsync(int id);

        Task<Product> AddAsync(Product product);

        Task UpdateAsync(Product product);

        Task DeleteAsync(int id);
    };
}
