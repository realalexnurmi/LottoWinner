using System.Globalization;
using System.Numerics;

namespace LottoWinner
{
	internal class Program
	{
		static void Main(string[] args)
		{
			IFieldProperty fieldProperty = new DefaultRulesFieldProperty(9);
			LottoGame game = new LottoGame(fieldProperty);

			for (int i = 0; i < 10; i++)
			{
				LottoTicket ticket = game.GenerateTicket();
				ticket.Print();
			}
		}
	}
}
