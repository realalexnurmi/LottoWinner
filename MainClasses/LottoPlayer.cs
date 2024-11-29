using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoWinner
{
	public class LottoPlayer
	{
		private ITicketStrategy strategy;
		private List<LottoTicket> tickets = new List<LottoTicket>();
		private LottoGame game;
		public int PlayerNumber { get; private set; }

		public LottoPlayer(LottoGame game, ITicketStrategy strategy)
		{
			this.game = game;
			this.strategy = strategy;
			if (strategy is SpecificNumbersTicketStrategy specificStrategy)
			{
				var invalidNumbers = specificStrategy.GetInvalidNumbers(1, game.FieldProperty.CountOfNumbers);

				foreach (var number in invalidNumbers)
				{
					specificStrategy.Forgot(number);
					Console.WriteLine($"Player № {PlayerNumber} forgot {number} in this game.");
				}

				if (specificStrategy.IsEmpty())
				{
					specificStrategy.SetNumbers(RandomLovedNumbers());
				}
			}
			game.AddPlayer(this);
		}

		public void SetNumber(int number)
		{
			PlayerNumber = number;
		}

		public List<int> RandomLovedNumbers()
		{
			var random = new Random();
			int countOfNumbers = game.FieldProperty.CountOfNumbers;
			int countOfLovedNumbers = random.Next(1, 8); // от 1 до 7
			var lovedNumbers = new HashSet<int>();

			while (lovedNumbers.Count < countOfLovedNumbers)
			{
				int number = random.Next(1, countOfNumbers + 1);
				lovedNumbers.Add(number);
			}

			return lovedNumbers.ToList();
		}

		public void GetTicket()
		{
			LottoTicket ticket = game.GenerateTicket(PlayerNumber, tickets.Count);
			while (!strategy.IsRightTicket(ticket))
			{
				ticket = game.GenerateTicket(PlayerNumber, tickets.Count);
			}
			tickets.Add(ticket);
		}

		public void GetNTickets(int n)
		{
			for (int i = 0; i < n; i++)
			{
				GetTicket();
			}
		}

		public void ShowTickets()
		{
			Console.WriteLine($"Player № {PlayerNumber} ({strategy.StrategyName}) has the following tickets:");

			if (strategy is SpecificNumbersTicketStrategy specificStrategy && !specificStrategy.IsEmpty())
			{
				Console.WriteLine($"Player № {PlayerNumber} loves the following numbers: {specificStrategy.ToString()}");
			}

			foreach (var ticket in tickets)
			{
				ticket.Print();
			}
		}

		public IReadOnlyList<LottoTicket> GetTickets()
		{
			return tickets.AsReadOnly();
		}
	}
}
