using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoWinner
{
	public class AnyTicketStrategy : ITicketStrategy
	{
		public bool IsRightTicket(LottoTicket ticket)
		{
			return true;
		}
	}
}
