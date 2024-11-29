using System.Globalization;
using System.Numerics;

namespace LottoWinner
{
	internal class Program
	{
		public static void Main(string[] args)
		{
			LottoGame game = new LottoGame(new DefaultRulesFieldProperty(9));

			LottoPlayer player1 = new LottoPlayer(game, new MinRankTicketStrategy(1));
			LottoPlayer player2 = new LottoPlayer(game, new SpecificNumbersTicketStrategy());
			LottoPlayer player3 = new LottoPlayer(game, new SpecificNumbersTicketStrategy(new List<int> { 5, 10, 15 }));
			LottoPlayer player4 = new LottoPlayer(game, new AnyTicketStrategy());

			player1.GetNTickets(3);
			player2.GetNTickets(3);
			player3.GetNTickets(3);
			player4.GetNTickets(3);

			game.ShowPlayers();
		}
	}
}
