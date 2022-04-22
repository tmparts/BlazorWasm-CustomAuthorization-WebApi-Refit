namespace MetaLib.Models
{
    public class UserSessionsPaginationResponseModel: PaginationResponseModel
    {
        public UserSessionModel[] Sessions { get; set; }
    }
}
