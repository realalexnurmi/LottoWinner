﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoWinner
{
	public interface ITicketStrategy
	{
		bool IsRightTicket(LottoTicket ticket);
		string StrategyName { get; }
	}
}
