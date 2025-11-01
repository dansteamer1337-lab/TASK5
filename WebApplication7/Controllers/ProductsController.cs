using Microsoft.AspNetCore.Mvc;
using WebApplication7.Models;
using WebApplication7.Repositories.Interfaces;

namespace WebApplication7.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts(
            [FromQuery] int page = 1,
            [FromQuery] decimal minPrice = 0,
            [FromQuery] decimal maxPrice = decimal.MaxValue)
        {
            if (page < 1) page = 1;
            if (minPrice < 0) minPrice = 0;
            if (maxPrice < minPrice) maxPrice = minPrice;

            var products = await _productRepository.GetProductsAsync(page, minPrice, maxPrice);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            if (string.IsNullOrEmpty(product.Name) || product.Price < 0)
            {
                return BadRequest("Name is required and Price must be non-negative");
            }

            try
            {
                await _productRepository.AddAsync(product);
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating product: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest("ID mismatch");
            }

            try
            {
                await _productRepository.UpdateAsync(product);
                return NoContent();
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating product: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await _productRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting product: {ex.Message}");
            }
        }
    }
}