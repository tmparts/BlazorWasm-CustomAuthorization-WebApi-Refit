using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaLib.Models
{
    public class UserSessionModel
    {
        public DateTime DateTimeStart { get; set; }

        public string IPAddressClient { get; set; }

        public string GuidTokenSession { get; set; }
    }
}
