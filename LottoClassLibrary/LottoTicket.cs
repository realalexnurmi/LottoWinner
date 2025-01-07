using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoClassLibrary
{
	public class LottoTicket
	{
		public string TicketNumber { get; private set; }
		public LottoField FirstField { get; private set; }
		public LottoField SecondField { get; private set;}
		public LottoTicket(string ticketNumber, LottoField firstField, LottoField secondField)
		{
			TicketNumber = ticketNumber;
			FirstField = firstField;
			SecondField = secondField;
		}
		public void PrintTicket()
		{
			Console.WriteLine($"Билет №{TicketNumber}");
			PrintField(FirstField);
			Console.WriteLine();
			PrintField(SecondField);
		}

		private void PrintField(LottoField field)
		{
			// Верхняя граница
			Console.WriteLine("+----+----+----+----+----+----+----+----+----+");

			for (int row = 1; row <= 3; row++)
			{
				Console.Write("|");
				for (int i = 0; i < 9; i++)
				{
					var column = field.Columns[i];
					if (column.NumberOne.Row == row)
					{
						Console.Write($" {column.NumberOne.Value,2} ");
					}
					else if (column.NumberTwo != null && column.NumberTwo.Row == row)
					{
						Console.Write($" {column.NumberTwo.Value,2} ");
					}
					else
					{
						Console.Write("    ");
					}
					Console.Write("|");
				}
				Console.WriteLine();
				// Нижняя граница для строк
				Console.WriteLine("+----+----+----+----+----+----+----+----+----+");
			}
		}
	}
}
