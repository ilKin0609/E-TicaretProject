using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.ReviewAndCommentDtos;
using E_Ticaret_Project.Application.DTOs.UserAuthenticationDtos;
using E_Ticaret_Project.Application.Shared.Responses;
using E_Ticaret_Project.Persistence.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace E_Ticaret_Project.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReviewAndCommentsController : ControllerBase
    {
        private IReviewAndCommentService _commentService { get; }

        public ReviewAndCommentsController(IReviewAndCommentService commentService)
        {
            _commentService = commentService;
        }
        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<ReviewAndCommentGetDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> GetProductOfComment([FromQuery] Guid productId)
        {
            var result = await _commentService.GetByProductIdAsync(productId);
            return StatusCode((int)result.StatusCode, result);
        }

        // POST api/<ReviewAndCommentsController>
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> AddComment([FromBody] ReviewAndCommentCreateDto dto)
        {
            var result=await _commentService.AddComment(dto);
            return StatusCode((int)result.StatusCode, result);
        }

        // DELETE api/<ReviewAndCommentsController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> RemoveComment(Guid id)
        {
            var result= await _commentService.RemoveComment(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
