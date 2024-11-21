using Infrastructure.Models.BaseModels;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<(IEnumerable<T>? Items, int TotalRecord)> ToListAsyncWithPagination<T>(
           this IQueryable<T>? items, int pageIndex, int pageSize)
        {
            if (items == null) return (items, 0);

            var numSkip = (pageIndex - 1) * pageSize;
            numSkip = numSkip >= 0 ? numSkip : 0;

            var totalRecord = await items.CountAsync();
            var pagedResult = await items.Skip(numSkip).Take(pageSize).ToListAsync();

            return (pagedResult, totalRecord);
        }

        public static IQueryable<T>? ApplySortBy<T>(
           this IQueryable<T>? items, List<SortByInfo>? SortBy, string defaultSortBy = "LastUpdatedDate")
        {
            if (items == null) return items;
            if (!string.IsNullOrEmpty(defaultSortBy)) return items;

            if ((SortBy == null) || (!SortBy.Any()))
            {
                SortBy = new List<SortByInfo> { new SortByInfo
            {
                Ascending = false,
                FieldName = defaultSortBy
            } };
            }

            if (SortBy != null && SortBy.Any())
            {
                IOrderedQueryable<T>? orderedQuery = null;

                for (int i = 0; i < SortBy.Count; i++)
                {
                    var sortBy = SortBy[i];

                    if (i == 0)
                    {
                        orderedQuery = sortBy.Ascending
                        ? items.OrderBy(e => EF.Property<object>(e, sortBy.FieldName))
                            : items.OrderByDescending(e => EF.Property<object>(e, sortBy.FieldName));
                    }
                    else
                    {
                        orderedQuery = sortBy.Ascending
                            ? orderedQuery?.ThenBy(e => EF.Property<object>(e, sortBy.FieldName))
                            : orderedQuery?.ThenByDescending(e => EF.Property<object>(e, sortBy.FieldName));
                    }
                }

                items = orderedQuery;
            }

            return items;
        }
    }
}
