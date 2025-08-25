using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.ProductDtos;
using E_Ticaret_Project.Application.Shared.Permissions;
using E_Ticaret_Project.Application.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
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


        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<ProductDetailDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetDetailById([FromQuery] Guid id)
        {
            var result = await _productService.GetDetailByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        // GET: /api/Products/GetDetailBySid?sid=XXXX
        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<ProductDetailDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetDetailBySid([FromQuery] string sid)
        {
            var result = await _productService.GetDetailBySidAsync(sid);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<ProductCardDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetByCategory([FromQuery] Guid categoryId)
        {
            var result = await _productService.GetByCategoryAsync(categoryId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<ProductCardDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetByProductCode([FromQuery] string ProductCode)
        {
            var result = await _productService.GetBySKUAsync(ProductCode);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<ProductCardDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetBySearch([FromQuery] string search)
        {
            var result = await _productService.SearchAsync(search);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<ProductImageDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetImages([FromQuery] Guid productId)
        {
            var r = await _productService.GetImagesAsync(productId);
            return StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<ProductImageDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetMainImage([FromQuery] Guid productId)
        {
            var r = await _productService.GetMainImageAsync(productId);
            return StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<ProductCardDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetByTagId([FromQuery] Guid tagId)
        {
            var result = await _productService.GetByTagId(tagId);
            return StatusCode((int)result.StatusCode, result);
        }


        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<ProductCardDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SearchByTag([FromQuery] string Tag)
        {
            var result = await _productService.GetByTagAsync(Tag);
            return StatusCode((int)result.StatusCode, result);
        }


        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<ProductCardDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SearchByManyTag([FromQuery] List<string> Tags)
        {
            var result = await _productService.GetByTagsAsync(Tags);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<ProductCardDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetAll([FromQuery] int size = 40)
        {
            var result = await _productService.GetAllAsync(size);
            return StatusCode((int)result.StatusCode, result);
        }


        [HttpPost]
        [Authorize(Policy = Permission.Product.Create)]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CreateProduct([FromForm] ProductCreateDto dto)
        {
            var result = await _productService.CreateAsync(dto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        [Authorize(Policy = Permission.Product.UploadMainImage)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UploadMainImage([FromForm] ProductMainImageUploadDto dto)
        {
            var r = await _productService.UploadMainImageAsync(dto);
            return StatusCode((int)r.StatusCode, r);
        }

        // --- UPLOAD ADDITIONAL ---
        [HttpPost]
        [Authorize(Policy = Permission.Product.UploadAdditionalImage)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UploadAdditionalImage([FromForm] ProductAdditionalImageUploadDto dto)
        {
            var r = await _productService.UploadAdditionalImageAsync(dto);
            return StatusCode((int)r.StatusCode, r);
        }


        [HttpPut]
        [Authorize(Policy = Permission.Product.SetMain)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SetMainImage([FromBody] SetMainDto dto)
        {
            var r = await _productService.SetMainImageAsync(dto);
            return StatusCode((int)r.StatusCode, r);
        }

        [HttpPut]
        [Authorize(Policy = Permission.Product.ReorderImage)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> ReorderImages([FromBody]ListedReorderDto dto)
        {
            var r = await _productService.ReorderImagesAsync(dto);
            return StatusCode((int)r.StatusCode, r);
        }

        [HttpPut]
        [Authorize(Policy = Permission.Product.UpdateImageAlt)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UpdateImageAlt([FromBody]  ProductUpdateAltDto dto)
        {
            var r = await _productService.UpdateImageAltAsync(dto);
            return StatusCode((int)r.StatusCode, r);
        }

        [HttpPut]
        [Authorize(Policy = Permission.Product.Update)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Update([FromBody] ProductUpdateDto dto)
        {
            var result = await _productService.UpdateAsync(dto);
            return StatusCode((int)result.StatusCode, result);
        }


        [HttpDelete]
        [Authorize(Policy = Permission.Product.RemoveImage)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> RemoveImage([FromQuery] Guid imageId)
        {
            var r = await _productService.RemoveImageAsync(imageId);
            return StatusCode((int)r.StatusCode, r);
        }

        [HttpDelete]
        [Authorize(Policy = Permission.Product.Delete)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> HardDelete([FromQuery] Guid id)
        {
            var result = await _productService.DeleteAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

    }
}
