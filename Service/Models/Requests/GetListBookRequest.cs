using Infrastructure.Models.BaseModels;

namespace Service.Models.Requests
{
    public class GetListBookRequest
    {
        public string? TextSearch { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; } = 0;
        public List<SortByInfo>? SortBy { get; set; }
    }
}
