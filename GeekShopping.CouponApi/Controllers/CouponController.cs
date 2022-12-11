using GeekShopping.CouponApi.Models.ValueObjects;
using GeekShopping.CouponApi.Repository.IRepository;
using GeekShopping.CouponApi.Utils;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.CouponApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase {
        
        private readonly ICouponRepository _repository;

        public CouponController(ICouponRepository repository) {
            _repository = repository;
        }

        [HttpGet("{code}")]
        [Authorize]
        public async Task<ActionResult<CouponVo>> FindByCode(string code)
        {
            CouponVo? coupon = await _repository.GetCouponByCode(code);
            if (coupon == null) return NotFound();
            return Ok(coupon);
        }

        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<ActionResult<CouponVo>> Create([FromBody]CouponVo vo)
        {
            CouponVo coupon = await _repository.RegisterNewCoupon(vo);
            if (coupon != null) return Ok(coupon);
            return BadRequest();
        }
        
        [HttpDelete("set-invalid/{id:long}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<ActionResult<CouponVo>> Delete(long id)
        {
            bool resp = await _repository.DeleteCoupon(id);
            if (resp) return Ok();
            return NotFound();
        }
    }
}
