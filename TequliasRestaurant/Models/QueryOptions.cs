using System.Linq.Expressions;

namespace TequliasRestaurant.Models
{
    public class QueryOptions<T> where T : class
    {
        public Expression<Func<T, Object>> OrderBy { get; set; } = null!;
        public Expression<Func<T, Object>> Where { get; set; } = null!;
        public string[] includes = Array.Empty<string>();
        public string Includes
        {
            set => includes = value.Replace(" ", "").Split(',');
        }

        public string[] GetIncludes() => includes;

        public bool HashWhere() => Where != null;
        public bool HasOrderBy() => OrderBy != null;

    }
}