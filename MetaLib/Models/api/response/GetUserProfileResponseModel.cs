﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace MetaLib.Models
{
    public class GetUserProfileResponseModel : ResponseBaseModel
    {
        public UserMediumModel User { get; set; }
    }
}