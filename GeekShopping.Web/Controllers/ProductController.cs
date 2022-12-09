using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;
using GeekShopping.Web.Utils;
using Microsoft.AspNetCore.Authentication;
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
            string? token = await HttpContext.GetTokenAsync("access_token") ?? "";
            IEnumerable<ProductViewModel>? products = await _productService.FindAllProductsAsync(token);

            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (!ModelState.IsValid) return View();
            string? token = await HttpContext.GetTokenAsync("access_token") ?? "";
            ProductViewModel? resp = await _productService.CrateProductAsync(model, token);
            if (resp != null) return RedirectToAction(nameof(Index));
            return View();
        }

        public async Task<IActionResult> Update(long id)
        {
            string? token = await HttpContext.GetTokenAsync("access_token") ?? "";
            ProductViewModel productViewModel = await _productService.FindProductByIdAsync(id, token) ?? new ProductViewModel();
            if (productViewModel.Id > 0) return View(productViewModel);
            return NotFound();
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update(ProductViewModel model)
        {
            if (!ModelState.IsValid) return View();
            string? token = await HttpContext.GetTokenAsync("access_token") ?? "";
            ProductViewModel? product = await _productService.UpdateProductAsync(model, token);
            if (product != null) return RedirectToAction(nameof(Index));
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Delete(long id)
        {
            string? token = await HttpContext.GetTokenAsync("access_token") ?? "";
            ProductViewModel productViewModel = await _productService.FindProductByIdAsync(id, token) ?? new ProductViewModel();
            if (productViewModel.Id > 0) return View(productViewModel);
            return NotFound();
        }
        
        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Delete(ProductViewModel model)
        {
            string? token = await HttpContext.GetTokenAsync("access_token") ?? "";
            bool resp = await _productService.DeleteProductAsync(model.Id, token);
            if (resp) return RedirectToAction(nameof(Index));
            return View();
        }
    }
}
