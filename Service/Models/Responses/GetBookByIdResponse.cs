using Infrastructure.Models.BaseModels;

namespace Service.Models.Responses
{
    public class GetBookByIdResponse
    {
        public GetBookByIdResponse(){ }
        public BookByIdResponse Book {  get; set; }

    }
    public class BookByIdResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public short? Status { get; set; }
        public short? ReadingStatus { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public DateTimeOffset? LastUpdatedDate { get; set; }
    }
}
