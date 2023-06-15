using Simpra.Schema.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Service.Service.Abstract
{
    public interface IBasketService
    {
        Task<BasketResponse> GetBasket(int userId);
        Task SaveOrUpdate(BasketRequest basketDto);
        Task Delete(int userId);
    }
}
