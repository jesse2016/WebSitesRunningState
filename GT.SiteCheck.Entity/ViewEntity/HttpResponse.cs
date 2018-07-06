using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GT.SiteCheck.Entity.ViewEntity
{
    public class HttpResponse<T>
    {
        public string StatusCode { set; get; }

        public string ErrorMsg { set; get; }

        public T Data { set; get; }
    }
}
