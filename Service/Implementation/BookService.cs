using AutoMapper;
using Azure;
using CoreInfrastructure.IRepository;
using CoreInfrastructure.Responses;
using Domains.Entities;
using Infrastructure.Common;
using Infrastructure.Helpers;
using Infrastructure.Models.BaseModels;
using Service.Constants;
using Service.IService;
using Service.Models.Requests;
using Service.Models.Responses;
using System.Linq.Expressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Service.Implementation
{
    public class BookService : GenericBackEndService, IBookService
    {
        private readonly IMapper _mapper;
        private readonly IBookRepository _bookRepository;
        public BookService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _bookRepository = Resolve<IBookRepository>();
            _mapper = Resolve<IMapper>();
        }

        public async Task<CreateBookResponse> CreateBook(CreateBookRequest request)
        {
            var response = new CreateBookResponse();
            var book = new Book()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false,
                Status = request.Status,
                ReadingStatus = request.ReadingStatus,
            };

            await _bookRepository.AddAsync(book);
            await _bookRepository.SaveChangesAsync();

            response.Data.Id = book.Id;
            response.IsSuccess = true;
            return response;
        }

        public async Task<BaseResponse<GetBookByIdResponse>> GetBookById(GetBookByIdRequest request)
        {
            var result = new BaseResponse<GetBookByIdResponse>();
            var resultData = new GetBookByIdResponse();
            var book = await _bookRepository.GetById(request.Id).ConfigureAwait(false);

            resultData.Book = new BookByIdResponse()
            {
                Id = book.Id,
                CreatedDate = book.CreatedDate,
                LastUpdatedDate = book.LastUpdatedDate,
                Name = book.Name,
                Status = book.Status,
                ReadingStatus = book.ReadingStatus
            };
            return result.BuildResult(resultData, MessageResponseConstant.MESSAGE_SUCCESS);
            
        }

        public async Task<BaseResponse<GetListBookResponse>> GetListBookAsync(GetListBookRequest request)
        {
            var result = new BaseResponse<GetListBookResponse>();
            var resultData = new GetListBookResponse();
            var expression = BuildFilterGetBooks(request);
            var data = await _bookRepository.GetAllBooks(expression, request.PageIndex, request.PageSize, request.SortBy);

            if (data == null)
            {
                resultData = new GetListBookResponse();
                return result;
            }
            resultData.Books = _mapper.Map<IEnumerable<BookResponse>>(data.ListDatas);
            return result.BuildResult(resultData, MessageResponseConstant.MESSAGE_SUCCESS);
        }

        public async Task<UpdateBookResponse> UpdateBook(UpdateBookRequest request)
        {
            var response = new UpdateBookResponse();
            var book = await _bookRepository.GetById(request.Id).ConfigureAwait(false);
            if (book == null)
            {
                response.IsSuccess = false;
                response.ErrorMessage = "Book not found";
            }

            _mapper.Map(request, book);
            book.LastUpdatedDate = DateTimeOffset.UtcNow;
            await _bookRepository.UpdateAsync(book).ConfigureAwait(false);
            await _bookRepository.SaveChangesAsync();

            response.Data.Id = book.Id;
            response.IsSuccess = true;
            return response;
        }

        #region Private method
        private Expression<Func<BookDbResponse, bool>> BuildFilterGetBooks(GetListBookRequest request)
        {
            Expression<Func<BookDbResponse, bool>> finalFilter = x => x.Name != null;

            if (!string.IsNullOrEmpty(request.TextSearch))
            {
                Expression<Func<BookDbResponse, bool>> filter = b => b.Name.ToLower().Contains(request.TextSearch.ToLower());
                finalFilter = finalFilter.And(filter);
            }

            return finalFilter;
        }
        #endregion
    }
}
