using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoWinner
{
	public class SpecificNumbersTicketStrategy : ITicketStrategy
	{
		private List<int> requiredNumbers;
		public string StrategyName => "Specific Numbers";

		public SpecificNumbersTicketStrategy()
		{
			this.requiredNumbers = new List<int>();
		}

		public SpecificNumbersTicketStrategy(List<int> requiredNumbers)
		{
			this.requiredNumbers = requiredNumbers.Distinct().ToList();
		}

		public bool IsRightTicket(LottoTicket ticket)
		{
			var allNumbers = ticket.Field1.Numbers.Concat(ticket.Field2.Numbers).ToList();
			return requiredNumbers.All(number => allNumbers.Contains(number));
		}

		public void Forgot(int number)
		{
			requiredNumbers.Remove(number);
		}

		public bool IsEmpty()
		{
			return requiredNumbers.Count == 0;
		}

		public void SetNumbers(List<int> numbers)
		{
			requiredNumbers = numbers.Distinct().ToList();
		}

		public List<int> GetInvalidNumbers(int start, int end)
		{
			return requiredNumbers.Where(number => number < start || number > end).ToList();
		}

		public override string ToString()
		{
			return string.Join(", ", requiredNumbers);
		}
	}
}
