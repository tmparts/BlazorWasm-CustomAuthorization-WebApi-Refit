using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaLib.Models.api.request
{
    public class ChangeUserProfileOptionsModel
    {
        public int UserId { get; set; }
        public string OptionAttribute { get; set; }
    }
}
