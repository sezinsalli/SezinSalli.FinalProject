using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Core.Entity
{
    
    public class Category : BaseEntity
    {

        //Kategori (adi. url, tags vs,) 
        public string Name { get; set; }
        public string url { get; set; }
        public string tag { get; set; }

        public ICollection<Product> Products { get; set; }

    }
}
