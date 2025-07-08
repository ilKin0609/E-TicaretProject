using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.ProductDtos;
using E_Ticaret_Project.Application.Shared.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace E_Ticaret_Project.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductService _productService { get; }
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        // GET: api/<ProductsController>
        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<ProductGetDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetAllProduct()
        {
            var result= await _productService.GetAllProduct();
            return StatusCode((int)result.StatusCode, result);
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponse<ProductGetDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetByIdProduct(Guid id)
        {
            var result = await _productService.GetByIdProduct(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<ProductGetDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetByTitleProduct([FromQuery] string title)
        {
            var result = await _productService.GetByTitleProduct(title);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<ProductGetDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetMyProducts([FromQuery] string userId)
        {
            var result = await _productService.GetMyProducts(userId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<ProductGetDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetDiscountProducts()
        {
            var result = await _productService.GetDiscountProducts();
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<ProductGetDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetByCategoryProducts([FromQuery] Guid categoryId)
        {
            var result = await _productService.GetByCategoryProducts(categoryId);
            return StatusCode((int)result.StatusCode, result);
        }


        // POST api/<ProductsController>
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CreateProduct([FromForm] ProductCreateDto dto)
        {
            var result = await _productService.CreateProduct(dto);
            return StatusCode((int)result.StatusCode, result);
        }

        // PUT api/<ProductsController>/5
        [HttpPut]
        [ProducesResponseType(typeof(BaseResponse<ProductUpdateDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductUpdateDto dto)
        {
            var result = await _productService.UpdateProduct(dto);
            return StatusCode((int)result.StatusCode, result);
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var result = await _productService.DeleteProduct(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
