////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace MetaLib.Models
{
    [Index(nameof(UserId))]
    [Index(nameof(ProjectId))]
    [Index(nameof(UserId), nameof(ProjectId))]
    public class UserToProjectLinkModelDb : IdModel
    {
        public int UserId { get; set; }
        public int ProjectId { get; set; }
    }
}