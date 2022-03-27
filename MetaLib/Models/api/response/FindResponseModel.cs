////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using MetaLib.Models;

namespace LibMetaApp.Models
{
    public class FindResponseModel : ResponseBaseModel
    {
        public PaginationResponseModel Pagination { get; set; }
    }
}