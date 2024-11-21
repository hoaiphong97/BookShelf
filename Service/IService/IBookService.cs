using Infrastructure.Models.BaseModels;
using Service.Models.Requests;
using Service.Models.Responses;

namespace Service.IService
{
    public interface IBookService
    {
        Task<BaseResponse<GetListBookResponse>> GetListBookAsync(GetListBookRequest request);
        Task<CreateBookResponse> CreateBook(CreateBookRequest request);
        Task<UpdateBookResponse> UpdateBook(UpdateBookRequest request);
        Task<BaseResponse<GetBookByIdResponse>> GetBookById(GetBookByIdRequest request);
    }
}
