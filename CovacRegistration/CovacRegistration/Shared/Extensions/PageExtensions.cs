using CovacRegistration.Shared.Models;

namespace CovacRegistration.Shared.Extensions
{
    public static class PageExtensions
	{
		public static PagedResult<T> GetPageDetails<T>(this IQueryable<T> query, int currentPage, int pageSize) where T : class
		{
			var result = new PagedResult<T>
			{
				CurrentPage = currentPage,
				PageSize = pageSize,
				RowCount = query.Count()
			};

			var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (currentPage - 1) * pageSize;
            result.Results = query.Skip(skip).Take(pageSize).ToList();

            return result;
        }
	}
}
