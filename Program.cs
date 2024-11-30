using System.Globalization;
using System.Numerics;

namespace LottoWinner
{
	internal class Program
	{
		public static void Main(string[] args)
		{
			LottoGame game = new LottoGame(new DefaultRulesFieldProperty(9));



			new LottoPlayer(game, new MinRankTicketStrategy(1)).SetDesiredTicketCount(5);
			new LottoPlayer(game, new AnyTicketStrategy()).SetDesiredTicketCount(5);
			new LottoPlayer(game, new SpecificNumbersTicketStrategy(
    () => player.RandomLovedNumbers(),
    (start, end) => player.GetInvalidNumbers(start, end),
    () => playerNumber));

			do
			{
                game.RunRuns(2000, 2, true);
            } while (Console.ReadLine() != "q");
				
        }
	}
}
