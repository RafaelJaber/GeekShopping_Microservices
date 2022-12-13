using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.Web.Controllers {
    public class CartController : Controller {

        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly ICouponService _couponService;

        public CartController(IProductService productService, ICartService cartService, ICouponService couponService)
        {
            _productService = productService;
            _cartService = cartService;
            _couponService = couponService;
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartViewModel model)
        {
            string token = await HttpContext.GetTokenAsync("access_token") ?? "";
            string userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value ?? "";

            bool response = await _cartService.ApplyCoupon(model, token);
            if (response) return RedirectToAction(nameof(Index));
            return RedirectToAction(nameof(Index));
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> RemoveCoupon()
        {
            string token = await HttpContext.GetTokenAsync("access_token") ?? "";
            string userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value ?? "";

            bool response = await _cartService.RemoveCoupon(userId, token);
            if (response) return RedirectToAction(nameof(Index));
            return RedirectToAction(nameof(Index));
        }
        
        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            return View(await FindUserCart());
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Checkout(CartViewModel model)
        {
            string token = await HttpContext.GetTokenAsync("access_token") ?? "";
            object? response = await _cartService.Checkout(model.CartHeader, token);
            if (response is string){
                TempData["Error"] = response;
                return RedirectToAction(nameof(Checkout));
            }
            if (response != null){
                return RedirectToAction(nameof(Confirmation));
            }
            return View(model);
        }
        
        [Authorize]
        public  IActionResult Confirmation()
        {
            return View();
        }

        private async Task<CartViewModel?> FindUserCart()
        {
            string token = await HttpContext.GetTokenAsync("access_token") ?? "";
            string userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value ?? "";

            CartViewModel? response = await _cartService.FindCartByUserId(userId, token);

            if (response?.CartHeader != null){
                if (!string.IsNullOrWhiteSpace(response.CartHeader.CuponCode)){
                    CouponViewModel? coupon = await _couponService.GetCouponByCode(response.CartHeader.CuponCode, token);
                    if (coupon?.CouponCode != null){
                        response.CartHeader.DiscountTotal = (double)coupon.DiscountAmount;
                    }
                }
                foreach (CartDetailViewModel item in response.CartDetails){
                    response.CartHeader.PurchaseAmount += (double)(item.Product.Price * item.Count);
                }
                response.CartHeader.PurchaseAmount -= response.CartHeader.DiscountTotal;
            }
            return response;
        }
    }
}
