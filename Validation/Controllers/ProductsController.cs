using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Validation.Data;
using Validation.Dtos;
using Validation.Models;

namespace Validation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("Add New Product")]
        public async Task<IActionResult> AddProduct(CreateEditProductDto productDto,
            [FromServices]IValidator<CreateEditProductDto> validator) {

            var ValidationResult = validator.Validate(productDto);
            if (!ValidationResult.IsValid)
            {
                var modelState = new ModelStateDictionary();
                ValidationResult.Errors.ForEach(error =>
                {
                    modelState.AddModelError(error.PropertyName, error.ErrorMessage);
                });
                return ValidationProblem(modelState);
            }
            var product = productDto.Adapt<Product>();
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return Ok(product);
        }

        [HttpGet("Get All Products")]

        public async Task<IActionResult> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            var response = products.Adapt<IEnumerable<GetProductDto>>();
            return Ok(response);
        }

        [HttpGet("Get Single Product By ID")]

        public async Task<IActionResult> GetProductById(int id) {
            var Product = await _context.Products.FindAsync(id);
            var response = Product.Adapt<GetProductDto>();
            return Ok(response);
        }

        [HttpPut("Update Product")]

        public async Task<IActionResult> EditProduct(CreateEditProductDto productDto, int id) {
            var Product = await _context.Products.FindAsync(id);
            Product.Name = productDto.Name;
            Product.Description = productDto.Description;
            Product.Price = productDto.Price;
            await _context.SaveChangesAsync();
            return Ok(productDto);
        }

        [HttpDelete("Delete Product")]
        public async Task<IActionResult> DeleteProduct(int id) {
            var Product = _context.Products.FindAsync(id);
            _context.Products.Remove(await Product);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
