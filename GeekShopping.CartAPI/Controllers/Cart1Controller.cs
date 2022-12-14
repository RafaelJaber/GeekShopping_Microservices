using GeekShopping.CartAPI.Messages;
using GeekShopping.CartAPI.Models.ValueObjects;
using GeekShopping.CartAPI.RabbitMQSender;
using GeekShopping.CartAPI.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace GeekShopping.CartAPI.Controllers
{
    [Route("api/v1/Cart")]
    [ApiController]
    public class Cart1Controller : ControllerBase {
        private readonly ICartRepository _cartRepository;
        private readonly ICouponRepository _couponRepository;
        private readonly IRabbitMqMessageSender _rabbitMqMessageSender;


        public Cart1Controller(ICartRepository cartRepository, IRabbitMqMessageSender rabbitMqMessageSender, ICouponRepository couponRepository)
        {
            _cartRepository = cartRepository;
            _couponRepository = couponRepository;
            _rabbitMqMessageSender = rabbitMqMessageSender ?? throw new ArgumentNullException(nameof(rabbitMqMessageSender));
        }
        
        
        [HttpGet("find-cart/{userId}")]
        //[Authorize]
        public async Task<ActionResult<CartVo>> FindById(string userId)
        {
            CartVo cart = await _cartRepository.FindCartByUserId(userId);
            if (cart == null) return NotFound();
            return Ok(cart);

        }
        
        [HttpPost("add-cart")]
        [Authorize]
        public async Task<ActionResult<CartVo>> AddCart(CartVo vo)
        {
            CartVo cart = await _cartRepository.SaveOrUpdateCart(vo);
            if (cart == null) return NotFound();
            return Ok(cart);

        }
        
        [HttpPut("update-cart")]
        [Authorize]
        public async Task<ActionResult<CartVo>> UpdateCart(CartVo vo)
        {
            CartVo cart = await _cartRepository.SaveOrUpdateCart(vo);
            if (cart == null) return NotFound();
            return Ok(cart);

        }
        
        [HttpDelete("remove-cart/{id:long}")]
        [Authorize]
        public async Task<ActionResult<CartVo>> RemoveCart(long id)
        {
            bool status = await _cartRepository.RemoveFromCart(id);
            if (!status) return NotFound();
            return Ok(true.ToString().ToLower());

        }
        
        [HttpPost("apply-coupon")]
        [Authorize]
        public async Task<ActionResult<CartVo>> ApplyCoupon(CartVo vo)
        {
            bool status = await _cartRepository.ApplyCoupon(vo.CartHeader.UserId, vo.CartHeader.CuponCode);
            if (!status) return NotFound();
            return Ok(true.ToString().ToLower());

        }
        
        [HttpDelete("remove-coupon/{userId}")]
        [Authorize]
        public async Task<ActionResult<CartVo>> RemoveCoupon(string userId)
        {
            bool status = await _cartRepository.RemoveCoupon(userId);
            if (!status) return NotFound();
            return Ok(true.ToString().ToLower());

        }
        
        [HttpPost("checkout")]
        [Authorize]
        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        public async Task<ActionResult<CheckoutHeaderVo>> Checkout(CheckoutHeaderVo vo)
        {
            string token = Request.Headers["Authorization"];
            
            if (vo?.UserId == null) return BadRequest();
            CartVo cart = await _cartRepository.FindCartByUserId(vo.UserId);
            if (cart == null) return NotFound();

            if (!string.IsNullOrEmpty(vo.CuponCode)){
                CouponVo coupon = await _couponRepository.GetCouponByCode(vo.CuponCode, token);
                if (vo.DiscountTotal != (double)coupon.DiscountAmount){
                    return StatusCode(412);
                } 
            }
            
            vo.CartDetails = cart.CartDetails;
            vo.DateTime = DateTime.Today;
            
            //RabbitMQ logic here!!
            _rabbitMqMessageSender.SendMessage(vo, "checkoutQueue");

            await _cartRepository.ClearCart(vo.UserId);
            
            return Ok(vo);

        }
    }
}
