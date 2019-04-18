using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerceiverACL
{
    public class apiACL
    {
        public string macAddr { get; set; }
        public string UserName { get; set; }
        public string[] AuthorizedModule { get; set; }
    }
}
