using Infrastructure.Models.BaseModels;

namespace CoreInfrastructure.Responses
{
    public class GetListBookDbResponse : BasePagination
    {
        public GetListBookDbResponse() { }
        public List<BookDbResponse> ListDatas { get; set; }
    }
    public class BookDbResponse
    {
        public string? Name {  get; set; }
        public short? Status {  get; set; }
        public DateTimeOffset? CreatedDate {  get; set; } 
        public DateTimeOffset? LastUpdatedDate {  get; set; } 
    }
}
