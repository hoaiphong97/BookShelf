using Infrastructure.Models.BaseModels;

namespace Service.Models.Responses
{
    public class GetListBookResponse : BasePagination
    {
        public GetListBookResponse() { }
        public IEnumerable<BookResponse> Books { get; set; }
    }
    public class BookResponse
    {
        public string? Name { get; set; }
        public short? Status { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public DateTimeOffset? LastUpdatedDate { get; set; }
    }
}
