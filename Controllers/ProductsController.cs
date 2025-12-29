using Median.Intranet.DAL.Contracts;
using Median.Intranet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Median.Intranet.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : BaseController
    {
        private readonly IProductEntityRepository productRepository;
        public ProductsController(IProductEntityRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var productResult = await productRepository.GetByIdAsync(id);            
            return FromResult(productResult);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var productsResult = await productRepository.GetAllAsync();            
            return FromResult<List<ProductEntity>>(productsResult);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request)
        {
            var productEntity = new Median.Intranet.Models.ProductEntity
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Tags = request.Tags
            };
            var createResult = await productRepository.CreateAsync(productEntity);            
            return FromResult(createResult);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductRequest request)
        {
            var existingProductResult = await productRepository.GetByIdAsync(id);
            if (!existingProductResult.Success)
            {
                return FromResult(existingProductResult);
            }
            var existingProduct = existingProductResult.Value;
            existingProduct.Name = request.Name;
            existingProduct.Description = request.Description;
            existingProduct.Price = request.Price;
            existingProduct.Tags = request.Tags;
            existingProduct.ShortDescription = request.ShortDescription;
            var updateResult = await productRepository.UpdateAsync(existingProduct);
            return FromResult(updateResult);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var result = await this.productRepository.DeleteAsync(id);
            return FromResult(result);
        }
    }    

    public class CreateProductRequest
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; }= 0;
        public string? Tags { get; set; }
    }
    public class UpdateProductRequest
    {        
        public string Name { get; set; }
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; } = 0;
        public string? Tags { get; set; }
    }
}
