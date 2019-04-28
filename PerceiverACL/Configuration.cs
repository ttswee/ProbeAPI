using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace PerceiverACL
{
    public class perceiverACL
    {
        public string UserName { get; set; }
        public string[] AuthorizedModule { get; set; }
    }

    public class apiACL
    {
        public List<perceiverACL> AccessRights { get; set; }
    }



}
