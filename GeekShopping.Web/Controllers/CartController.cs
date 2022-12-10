using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.Web.Controllers {
    public class CartController : Controller {

        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public CartController(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await FindUserCart());
        }

        [Authorize]
        public async Task<IActionResult> Remove(long id)
        {
            string token = await HttpContext.GetTokenAsync("access_token") ?? "";
            string userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value ?? "";

            bool response = await _cartService.RemoveFromCart(id, token);
            if (response) return RedirectToAction(nameof(Index));
            return RedirectToAction(nameof(Index));
        }

        private async Task<CartViewModel?> FindUserCart()
        {
            string token = await HttpContext.GetTokenAsync("access_token") ?? "";
            string userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value ?? "";

            CartViewModel? response = await _cartService.FindCartByUserId(userId, token);

            if (response?.CartHeader != null){
                foreach (CartDetailViewModel item in response.CartDetails){
                    response.CartHeader.PurchaseAmount += (double)(item.Product.Price * item.Count);
                }
            }
            return response;
        }
    }
}
