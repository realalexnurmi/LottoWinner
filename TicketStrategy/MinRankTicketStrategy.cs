using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoWinner
{
	public class MinRankTicketStrategy : ITicketStrategy
	{
		private int minRank;
		public string StrategyName => "Min Rank";

		public MinRankTicketStrategy(int minRank)
		{
			this.minRank = minRank;
		}

		public bool IsRightTicket(LottoTicket ticket)
		{
			return ticket.Rank <= minRank;
		}
	}
}
