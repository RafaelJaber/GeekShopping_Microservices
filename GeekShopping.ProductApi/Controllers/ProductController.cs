using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GeekShopping.ProductApi.Repository.IRepository;
using GeekShopping.ProductApi.Models.ValueObjects;

namespace GeekShopping.ProductApi.Controllers {
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase {
        private readonly IProductRepository _repository;


        public ProductController(IProductRepository repository)
        {
            _repository = repository
                          ?? throw new
                              ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductVo>>> FindAll()
        {
            IEnumerable<ProductVo> products = await _repository.FindAllAsync();
            return Ok(products);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult> FindById(long id)
        {
            ProductVo product = await _repository.FindByIdAsync(id);
            if (product.Id <= 0) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductVo>> Create([FromBody] ProductVo vo)
        {
            if (vo == null) return BadRequest();
            var product = await _repository.CrateAsync(vo);
            return Ok(vo);
        }

        [HttpPut]
        public async Task<ActionResult<ProductVo>> Update([FromBody] ProductVo vo)
        {
            if (vo == null) return BadRequest();
            ProductVo product = await _repository.UpdateAsync(vo);
            if (product.Id <= 0) return BadRequest();
            return product;
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(long id)
        {
            bool status = await _repository.DeleteAsync(id);
            if (!status) return BadRequest();
            return Ok();
        }
    }
}
