using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoWinner.Interfaces
{
    public interface IInitializableTicketStrategy : ITicketStrategy
    {
        void Initialize();
    }
}
