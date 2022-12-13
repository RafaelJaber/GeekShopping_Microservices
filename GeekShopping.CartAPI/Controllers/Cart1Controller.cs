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

namespace GeekShopping.CartAPI.Controllers
{
    [Route("api/v1/Cart")]
    [ApiController]
    public class Cart1Controller : ControllerBase {
        private readonly ICartRepository _repository;
        private readonly IRabbitMqMessageSender _rabbitMqMessageSender;


        public Cart1Controller(ICartRepository repository, IRabbitMqMessageSender rabbitMqMessageSender)
        {
            _repository = repository;
            _rabbitMqMessageSender = rabbitMqMessageSender ?? throw new ArgumentNullException(nameof(rabbitMqMessageSender));
        }
        
        
        [HttpGet("find-cart/{userId}")]
        //[Authorize]
        public async Task<ActionResult<CartVo>> FindById(string userId)
        {
            CartVo cart = await _repository.FindCartByUserId(userId);
            if (cart == null) return NotFound();
            return Ok(cart);

        }
        
        [HttpPost("add-cart")]
        [Authorize]
        public async Task<ActionResult<CartVo>> AddCart(CartVo vo)
        {
            CartVo cart = await _repository.SaveOrUpdateCart(vo);
            if (cart == null) return NotFound();
            return Ok(cart);

        }
        
        [HttpPut("update-cart")]
        [Authorize]
        public async Task<ActionResult<CartVo>> UpdateCart(CartVo vo)
        {
            CartVo cart = await _repository.SaveOrUpdateCart(vo);
            if (cart == null) return NotFound();
            return Ok(cart);

        }
        
        [HttpDelete("remove-cart/{id:long}")]
        [Authorize]
        public async Task<ActionResult<CartVo>> RemoveCart(long id)
        {
            bool status = await _repository.RemoveFromCart(id);
            if (!status) return NotFound();
            return Ok(true.ToString().ToLower());

        }
        
        [HttpPost("apply-coupon")]
        [Authorize]
        public async Task<ActionResult<CartVo>> ApplyCoupon(CartVo vo)
        {
            bool status = await _repository.ApplyCoupon(vo.CartHeader.UserId, vo.CartHeader.CuponCode);
            if (!status) return NotFound();
            return Ok(true.ToString().ToLower());

        }
        
        [HttpDelete("remove-coupon/{userId}")]
        [Authorize]
        public async Task<ActionResult<CartVo>> RemoveCoupon(string userId)
        {
            bool status = await _repository.RemoveCoupon(userId);
            if (!status) return NotFound();
            return Ok(true.ToString().ToLower());

        }
        
        [HttpPost("checkout")]
        [Authorize]
        public async Task<ActionResult<CheckoutHeaderVo>> Checkout(CheckoutHeaderVo vo)
        {
            if (vo?.UserId == null) return BadRequest();
            CartVo cart = await _repository.FindCartByUserId(vo.UserId);
            if (cart == null) return NotFound();
            vo.CartDetails = cart.CartDetails;
            vo.DateTime = DateTime.Today;
            
            //RabbitMQ logic here!!
            _rabbitMqMessageSender.SendMessage(vo, "checkoutQueue");
            
            return Ok(vo);

        }
    }
}
