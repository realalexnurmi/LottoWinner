using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoWinner
{
	public class AnyTicketStrategy : ITicketStrategy
	{
		public string StrategyName => "Any Ticket";
		public bool IsRightTicket(LottoTicket ticket)
		{
			return true;
		}
	}
}
