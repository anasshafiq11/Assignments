using Assignment2.DTOs.Product;
using Assignment2.Models;
using Assignment2.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;


namespace ProductManagementApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _service;
        private readonly IMapper _mapper;

        public ProductController(
            IProductService service,
            IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET: /Product
        public IActionResult Index()
        {
            var products = _service.GetAll();
            return View(products);
        }

        // GET: /Product/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Product/Create
        [HttpPost]
        public IActionResult Create(ProductDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var product = _mapper.Map<Product>(dto);

            _service.Add(product);

            return RedirectToAction(nameof(Index));
        }

        // GET: /Product/Edit/1
        public IActionResult Edit(int id)
        {
            var product = _service.GetById(id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // POST: /Product/Edit
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            _service.Update(product);

            return RedirectToAction(nameof(Index));
        }

        // GET: /Product/Delete/1
        public IActionResult Delete(int id)
        {
            var product = _service.GetById(id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // POST: /Product/DeleteConfirmed
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            _service.Delete(id);

            return RedirectToAction(nameof(Index));
        }

        // GET: /Product/Details/1
        public IActionResult Details(int id)
        {
            var product = _service.GetById(id);

            if (product == null)
                return NotFound();

            return View(product);
        }
    }
}