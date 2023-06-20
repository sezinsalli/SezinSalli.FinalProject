using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Core.Enum
{
    public enum ProductStatus
    {
        InStock=1, // Ürün stokta bulunuyor.
        OutOfStock, // Ürün stokta bulunmuyor.
        Discontinued, // Ürünün üretimi durduruldu.
        None
    }
}
