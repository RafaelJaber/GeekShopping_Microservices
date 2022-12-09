using GeekShopping.CartAPI.Models.ValueObjects;
using GeekShopping.CartAPI.Repository.Interface;
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


        public Cart1Controller(ICartRepository repository) {
            _repository = repository;
        }

        [HttpGet("find-cart{userId:long}")]
        public async Task<ActionResult<CartVo>> FindById(string userId)
        {
            CartVo cart = await _repository.FindCartByUserId(userId);
            if (cart == null) return NotFound();
            return Ok(cart);

        }
        
        [HttpPost("add-cart/{id:long}")]
        public async Task<ActionResult<CartVo>> AddCart(CartVo vo)
        {
            CartVo cart = await _repository.SaveOrUpdateCart(vo);
            if (cart == null) return NotFound();
            return Ok(cart);

        }
        
        [HttpPut("update-cart/{id:long}")]
        public async Task<ActionResult<CartVo>> UpdateCart(CartVo vo)
        {
            CartVo cart = await _repository.SaveOrUpdateCart(vo);
            if (cart == null) return NotFound();
            return Ok(cart);

        }
        
        [HttpDelete("remove-cart/{id:long}")]
        public async Task<ActionResult<CartVo>> RemoveCart(long id)
        {
            bool status = await _repository.RemoveFromCart(id);
            if (!status) return NotFound();
            return Ok(true.ToString().ToLower());

        }
    }
}
