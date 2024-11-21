using Infrastructure.Models.BaseModels;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using Service.Models.Requests;
using Service.Models.Responses;

namespace BookShelfKimHa.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse<GetListBookResponse>>> GetListBookAsync(GetListBookRequest request)
        {
            var result = await _bookService.GetListBookAsync(request);
            return result != null ? (result.IsSuccess ? Ok(result) : BadRequest(result)) : BadRequest();
        }

        [HttpPost]
        public async Task<ActionResult<CreateBookResponse>> Create(CreateBookRequest request)
        {
            var result = await _bookService.CreateBook(request);
            return result != null ? (result.IsSuccess ? Ok(result) : BadRequest(result)) : BadRequest();

        }

        [HttpPut]
        public async Task<ActionResult<UpdateBookResponse>> Update(UpdateBookRequest request)
        {
            var result = await _bookService.UpdateBook(request);
            return result != null ? (result.IsSuccess ? Ok(result) : BadRequest(result)) : BadRequest();

        }

        [HttpPost]
        public async Task<ActionResult<GetBookByIdResponse>> GetById(GetBookByIdRequest request)
        {
            var result = await _bookService.GetBookById(request);
            return result != null ? (result.IsSuccess ? Ok(result) : BadRequest(result)) : BadRequest();

        }
    }
}
