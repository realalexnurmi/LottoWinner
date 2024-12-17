using System.Globalization;
using System.Numerics;

namespace LottoWinner
{
	internal class Program
	{
		public static void Main(string[] args)
		{
			LottoGame game = new LottoGame(new DefaultRulesFieldProperty(9));

			new LottoPlayer(game, new MinRankTicketStrategy(1)).SetDesiredTicketCount(10);
			new LottoPlayer(game, new AnyTicketStrategy()).SetDesiredTicketCount(1000);

			game.RunRuns(10, 1000);
		}
	}
}
