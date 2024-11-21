using Infrastructure.Models.BaseModels;

namespace CoreInfrastructure.Request
{
    public class GetListBookDbRequest
    {
        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; } = 0;
        public SortByInfo? SortBy { get; set; }
    }
}
