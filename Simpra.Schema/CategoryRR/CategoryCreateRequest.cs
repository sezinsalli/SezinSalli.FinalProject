using Simpra.Schema.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Schema.CategoryRR
{
    public class CategoryCreateRequest:BaseRequest
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Tag { get; set; }
    }
}
