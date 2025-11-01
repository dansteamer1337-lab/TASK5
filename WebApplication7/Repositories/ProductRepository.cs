using Microsoft.EntityFrameworkCore;
using WebApplication7.Data;
using WebApplication7.Models;
using WebApplication7.Repositories.Interfaces;

namespace WebApplication7.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private const int PageSize = 10;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetProductsAsync(int pageNumber, decimal minPrice, decimal maxPrice)
        {
            return await _context.Products
                .Where(p => !p.IsDeleted && p.Price >= minPrice && p.Price <= maxPrice)
                .OrderBy(p => p.Price)
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }

        public async Task AddAsync(Product product)
        {
            product.IsDeleted = false;
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            var existingProduct = await GetByIdAsync(product.Id);
            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;

                _context.Products.Update(existingProduct);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException($"Product with id {product.Id} not found");
            }
        }

        public async Task DeleteAsync(int id)
        {
            var product = await GetByIdAsync(id);
            if (product != null)
            {
                product.IsDeleted = true;
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException($"Product with id {id} not found");
            }
        }
    }
}