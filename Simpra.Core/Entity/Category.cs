﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Core.Entity
{
    
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Tag { get; set; }

        public ICollection<Product> Products { get; set; }

    }
}
