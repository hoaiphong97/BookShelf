using CoreInfrastructure.DataContext;
using CoreInfrastructure.IRepository;
using CoreInfrastructure.Request;
using CoreInfrastructure.Responses;
using Domains.Entities;
using Infrastructure.Models.BaseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistance.Extensions;
using System.Linq.Expressions;

namespace CoreInfrastructure.Repository
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        MyDataContext _dbContext;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BookRepository(MyDataContext dbContext, IHttpContextAccessor httpContextAccessor , IServiceProvider serviceProvider) : base(dbContext, httpContextAccessor, serviceProvider)
        {
            _dbContext = dbContext;
            _serviceProvider = serviceProvider;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetListBookDbResponse> GetAllBooks(Expression<Func<BookDbResponse, bool>> expression, int pageIndex, int pageSize, List<SortByInfo> sortBy)
        {
            var response = new GetListBookDbResponse();
            var books = BuildQueryBook(expression);

            // Apply sort
            books = books.ApplySortBy(sortBy);

            // Pagination
            books = BuildPagingQuery(books, pageIndex, pageSize);
            response.ListDatas = await books.ToListAsync();

            return response;
        }

        public async Task<int> CountFinanceAsync(Expression<Func<BookDbResponse, bool>> expression)
        {
            var query = BuildQueryBook(expression);
            return await query.CountAsync().ConfigureAwait(false);
        }

        public async Task<CreateOrUpdateBookDbResponse> CreateOrUpdateBookDbResponse(CreateBookDbRequest request)
        {
            var result = new CreateOrUpdateBookDbResponse();

            throw new NotImplementedException();
        }

        #region Private method
        private IQueryable<BookDbResponse> BuildQueryBook(Expression<Func<BookDbResponse, bool>> expression)
        {
            var query = from book in _dbContext.Books.AsNoTracking()
                        select new BookDbResponse()
                        {
                            Name = book.Name,
                            Status = book.Status,
                            CreatedDate = book.CreatedDate,
                            LastUpdatedDate = book.LastUpdatedDate,
                        };
            query = query.Where(expression);
            return query;
        }
        #endregion
    }
}
