using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoWinner.TicketStrategy
{
    public class CompositeTicketStrategy : ITicketStrategy
    {
        private readonly ITicketStrategy baseStrategy;
        private readonly ITicketStrategy additionalStrategy;

        public string StrategyName => $"Composite ({baseStrategy.StrategyName} + {additionalStrategy.StrategyName})";
        public CompositeTicketStrategy(ITicketStrategy baseStrategy, ITicketStrategy additionalStrategy)
        {
            this.baseStrategy = baseStrategy;
            this.additionalStrategy = additionalStrategy;
        }

        public bool IsRightTicket(LottoTicket ticket)
        {
            // Сначала вызываем базовую стратегию
            bool baseResult = baseStrategy.IsRightTicket(ticket);

            // Если базовая стратегия вернула true, выполняем дополнительные проверки
            if (baseResult)
            {
                return additionalStrategy.IsRightTicket(ticket);
            }

            return baseResult;
        }
    }
}
