using Assignment2.DTOs.Product;
using Assignment2.Models;
using Assignment2.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ProductManagementApp.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService _service;
        private readonly IAppInfoService _appInfo;
        private readonly IRequestTracker _requestTracker;
        private readonly IMapper _mapper;

        public ProductController(
            IProductService service,
            IAppInfoService appInfo,
            IRequestTracker requestTracker,
            IMapper mapper)
        {
            _service = service;
            _appInfo = appInfo;
            _requestTracker = requestTracker;
            _mapper = mapper;
        }

        // GET: /Product
        public async Task<IActionResult> Index()
        {
            ViewBag.ApplicationId = _appInfo.ApplicationId;
            ViewBag.StartTime = _appInfo.StartTime;
            ViewBag.RequestId = _requestTracker.RequestId;
            var products = await _service.GetAllAsync();

            return View(products);
        }

        // GET: /Product/Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: /Product/Create
        [HttpPost]
        public async Task<IActionResult> Create(ProductDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var product = _mapper.Map<Product>(dto);

            await _service.AddAsync(product);

            return RedirectToAction(nameof(Index));
        }

        // GET: /Product/Edit/1
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _service.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // POST: /Product/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            await _service.UpdateAsync(product);

            return RedirectToAction(nameof(Index));
        }

        // GET: /Product/Delete/1
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _service.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // POST: /Product/DeleteConfirmed
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }

        // GET: /Product/Details/1
        public async Task<IActionResult> Details(int id)
        {
            var product = await _service.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            return View(product);
        }
    }
}