using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Allegory.Entities.Concrete
{
    public class PagedResult<T> where T : class, new()
    {
        public IList<T> Results { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }

        public PagedResult()
        {
            Results = new List<T>();
        }

        public int FirstRowOnPage=> (CurrentPage - 1) * PageSize + 1;

        public int LastRowOnPage=> Math.Min(CurrentPage * PageSize, RowCount);
    }
}
