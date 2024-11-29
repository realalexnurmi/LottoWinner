using System;
using System.Collections.Generic;
using System.Linq;

namespace LottoWinner
{
	public class LottoTicket
	{
		public LottoField Field1 { get; private set; }
		public LottoField Field2 { get; private set; }
		public int Rank { get; private set; }
		public int? TicketNumber { get; set; }

		public LottoTicket(LottoField field1, LottoField field2)
		{
			Field1 = field1;
			Field2 = field2;
			TicketNumber = null;
			Rank = CalculateRank();
		}

		private int CalculateRank()
		{
			return Field1.Category.Rang * Field2.Category.Rang;
		}

		public void Print()
		{
			Console.WriteLine(new string('-', 30));
			Console.WriteLine($"Ticket № {TicketNumber ?? 0} Rang: {Rank}");
			Console.WriteLine("Field 1:");
			Field1.Print();
			Console.WriteLine("Field 2:");
			Field2.Print();
			Console.WriteLine(new string('-', 30));
		}
	}
}
