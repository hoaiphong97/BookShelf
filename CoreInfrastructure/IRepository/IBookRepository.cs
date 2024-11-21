using CoreInfrastructure.Request;
using CoreInfrastructure.Responses;
using Domains.Entities;
using Infrastructure.Models.BaseModels;
using System.Linq.Expressions;

namespace CoreInfrastructure.IRepository
{
    public interface IBookRepository : IBaseRepository<Book>
    {
        Task<CreateOrUpdateBookDbResponse> CreateOrUpdateBookDbResponse(CreateBookDbRequest request);
        Task<int> CountFinanceAsync(Expression<Func<BookDbResponse, bool>> expression);
        Task<GetListBookDbResponse> GetAllBooks(Expression<Func<BookDbResponse, bool>> expression, int pageIndex, int pageSize, List<SortByInfo> sortBy);
    }
}
