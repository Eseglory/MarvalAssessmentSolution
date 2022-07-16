using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Core.Helpers
{
    public class PagedList<T>
    {
        public int CurrentPage { get; set; }
        public int TotalRecords { get; set; }
        public List<T> Result { get; set; } = new List<T>();

    }
}
