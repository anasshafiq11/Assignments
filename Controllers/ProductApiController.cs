using Assignment2.DTOs.Product;
using Assignment2.Models;
using Assignment2.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;


namespace ProductManagementApp.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly IMapper _mapper;

        public ProductApiController(
            IProductService service,
            IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
       
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _service.GetAllAsync();
            return Ok(new ApiResponse<List<Product>>
            {
                Success = true,
                Message = "Products Retrieved",
                Data = products
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _service.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound(new ApiResponse<Product>
                {
                    Success = false,
                    Message = "Product Not Found"
                });
            }

            return Ok(new ApiResponse<Product>
            {
                Success = true,
                Message = "Product Found",
                Data = product
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductDto dto)
        {
            var product = _mapper.Map<Product>(dto);
            await _service.AddAsync(product);

            return Ok(new ApiResponse<Product>
            {
                Success = true,
                Message = "Product Created",
                Data = product
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductDto dto)
        {
            var product = _mapper.Map<Product>(dto);
            product.Id = id;

            await _service.UpdateAsync(product);

            return Ok(new ApiResponse<Product>
            {
                Success = true,
                Message = "Product Updated",
                Data = product
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Product Deleted",
                Data = null
            });
        }
    }
}