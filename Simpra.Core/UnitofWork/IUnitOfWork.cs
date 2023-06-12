using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Core.UnitofWork
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
        Task CompleteWithTransactionAsync();
    }
}
