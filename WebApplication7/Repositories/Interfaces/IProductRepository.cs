using WebApplication7.Models;

namespace WebApplication7.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProductsAsync(int pageNumber, decimal minPrice, decimal maxPrice);
        Task<Product?> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
    }
}