using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;
using GeekShopping.Web.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.Web.Controllers {
    public class ProductController : Controller {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Product>? products = await _productService.FindAllProductsAsync();

            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Product model)
        {
            if (!ModelState.IsValid) return View();
            Product? resp = await _productService.CrateProductAsync(model);
            if (resp != null) return RedirectToAction(nameof(Index));
            return View();
        }

        public async Task<IActionResult> Update(long id)
        {
            Product product = await _productService.FindProductByIdAsync(id) ?? new Product();
            if (product.Id > 0) return View(product);
            return NotFound();
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update(Product model)
        {
            if (!ModelState.IsValid) return View();
            Product? product = await _productService.UpdateProductAsync(model);
            if (product != null) return RedirectToAction(nameof(Index));
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Delete(long id)
        {
            Product product = await _productService.FindProductByIdAsync(id) ?? new Product();
            if (product.Id > 0) return View(product);
            return NotFound();
        }
        
        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Delete(Product model)
        {
            bool resp = await _productService.DeleteProductAsync(model.Id);
            if (resp) return RedirectToAction(nameof(Index));
            return View();
        }
    }
}
