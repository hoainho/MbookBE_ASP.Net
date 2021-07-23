using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mbook_ASP.Models
{
    public class Password
    {
        public string passwordOld { get; set; }
        public string passwordNew { get; set; }
        public string repeatpasswordNew { get; set; }
        public Password(string passwordOld, string passwordNew, string repeatpasswordNew)
        {
            this.passwordOld = passwordOld;
            this.passwordNew = passwordNew;
            this.repeatpasswordNew = repeatpasswordNew;
        }

        
    }
}