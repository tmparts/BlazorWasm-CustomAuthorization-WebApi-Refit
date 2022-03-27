using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaLib.Models
{
    public class PaginationRequestModel
    {
        public int PageSize { get; set; }
        public int PageNum { get; set; }
    }

    public class PaginationResponseModel : PaginationRequestModel
    {
        public int TotalCount { get; set; }
    }
}
