using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Core.Enum
{
    public enum OrderStatus
    {
        Pending=1, // Sipariş henüz işleme alınmadı.
        Processing, // Sipariş işleme alındı ve hazırlanıyor.
        Shipped, // Sipariş gönderildi.
        Delivered, // Sipariş teslim edildi.
        Cancelled, // Sipariş iptal edildi.
        Returned, // İade edildi
        OnHold, // Geçici bekleme durumu
        None
    }
}
