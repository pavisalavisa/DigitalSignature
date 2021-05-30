using System.Collections.Generic;

namespace Application.Common.Models
{
    public class PagingResultModel<T>
    {
        public int Count { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}