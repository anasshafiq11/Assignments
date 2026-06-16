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
        public IActionResult GetAll()
        {
            return Ok(new ApiResponse<List<Product>>
            {
                Success = true,
                Message = "Products Retrieved",
                Data = _service.GetAll()
            });
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = _service.GetById(id);

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
        public IActionResult Create(ProductDto dto)
        {
            var product = _mapper.Map<Product>(dto);

            _service.Add(product);

            return Ok(new ApiResponse<Product>
            {
                Success = true,
                Message = "Product Created",
                Data = product
            });
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, ProductDto dto)
        {
            var product = _mapper.Map<Product>(dto);
            product.Id = id;

            _service.Update(product);

            return Ok(new ApiResponse<Product>
            {
                Success = true,
                Message = "Product Updated",
                Data = product
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Product Deleted",
                Data = null
            });
        }
    }
}