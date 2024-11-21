namespace Infrastructure.Models.BaseModels
{
    public class BasePagination
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; } = 0;
        public long TotalRecord { get; set; } = 0;
    }
}
