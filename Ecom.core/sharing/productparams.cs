using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.core.sharing
{
    public class productparams
    {
        public string? Sort { get; set; } 

        public int? CategoryId { get; set; }
        public string? Search { get; set; }

        public int Maxpagesize { get; set; } = 6;
        private int _pagesize=3;
        public int pagesize
        {
            get { return _pagesize; }
            set { _pagesize = value >Maxpagesize?Maxpagesize:value; }

        }

        public int pageNumber { get; set; } = 1;

    }
}
