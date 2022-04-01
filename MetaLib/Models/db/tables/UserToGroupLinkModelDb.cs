////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace MetaLib.Models
{
    [Index(nameof(UserId))]
    [Index(nameof(GroupId))]
    [Index(nameof(UserId), nameof(GroupId))]
    public class UserToGroupLinkModelDb : IdModel
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }
    }
}