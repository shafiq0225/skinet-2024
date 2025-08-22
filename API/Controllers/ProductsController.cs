using API.RequestHelpers;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductsController(IUnitOfWork unit) : BaseApiController
    {

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts([FromQuery] ProductSpecParams specParams)
        {
            var spec = new ProductSpecification(specParams);

            return Ok(await CreatePagedResult(unit.Repository<Product>(), spec, specParams.PageIndex, specParams.PageSize));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await unit.Repository<Product>().GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            unit.Repository<Product>().Add(product);
            if (await unit.Complete())
            {
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }

            return BadRequest("Failed to create product");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id || !ProductExists(id)) return BadRequest("Cannot update this product");

            unit.Repository<Product>().Update(product);
            if (await unit.Complete())
            {
                return NoContent();
            }
            return BadRequest("Failed to update product");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await unit.Repository<Product>().GetByIdAsync(id);

            if (product == null) return NotFound();

            unit.Repository<Product>().Delete(product);
            if (await unit.Complete())
            {
                return NoContent();
            }
            return BadRequest("Failed to delete product");
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            var spec = new BrandListSpecification();
            return Ok(await unit.Repository<Product>().ListAsync(spec));
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            var spec = new TypeListSpecification();
            return Ok(await unit.Repository<Product>().ListAsync(spec));
        }

        private bool ProductExists(int id)
        {
            return unit.Repository<Product>().Exists(id);
        }
    }
}
