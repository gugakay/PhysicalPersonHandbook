namespace DataAccess.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Page<T>(this IQueryable<T> source, int? pageNumber, int? itemsPerPage)
        {
            if(source == null) throw new ArgumentNullException($"{nameof(T)}");

            var skip = (pageNumber - 1) * itemsPerPage;
            skip = skip > 0 ? skip : 0;

            var take = itemsPerPage > 0 ? itemsPerPage.Value : int.MaxValue;

            return source
                .Skip(skip.Value)
                .Take(take);
        }
    }
}
