using Microsoft.AspNetCore.Mvc;
using GeekShopping.ProductApi.Repository.IRepository;
using GeekShopping.ProductApi.Models.ValueObjects;
using GeekShopping.ProductApi.Utils;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize]
        public async Task<ActionResult<IEnumerable<ProductVo>>> FindAll()
        {
            IEnumerable<ProductVo> products = await _repository.FindAllAsync();
            return Ok(products);
        }

        [HttpGet("{id:long}")]
        [Authorize]
        public async Task<ActionResult> FindById(long id)
        {
            ProductVo product = await _repository.FindByIdAsync(id);
            if (product.Id <= 0) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ProductVo>> Create([FromBody] ProductVo vo)
        {
            if (vo == null) return BadRequest();
            var product = await _repository.CrateAsync(vo);
            return Ok(vo);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<ProductVo>> Update([FromBody] ProductVo vo)
        {
            if (vo == null) return BadRequest();
            ProductVo product = await _repository.UpdateAsync(vo);
            if (product.Id <= 0) return BadRequest();
            return product;
        }

        [HttpDelete("{id:long}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<ActionResult> Delete(long id)
        {
            bool status = await _repository.DeleteAsync(id);
            if (!status) return BadRequest();
            return Ok(status.ToString().ToLower());
        }
    }
}
