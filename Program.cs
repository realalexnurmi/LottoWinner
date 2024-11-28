using System.Globalization;
using System.Numerics;

namespace LottoWinner
{
	internal class Program
	{
		static void Main(string[] args)
		{
			IFillingStrategy fillingStrategy = new DefaultFillingStrategy();
			var lottoGame = new LottoGame(9, fillingStrategy);

			Console.WriteLine("Categories:");
			lottoGame.PrintCategories();
			Console.WriteLine("Total Valid Fields: " + lottoGame.CalculateTotalValidFields().ToString("F"));
			Console.WriteLine("Probability on first possible Win Step: " + ((double)lottoGame.CalculateWinningProbabilityOnFirstWinStep()).ToString("P3", CultureInfo.InvariantCulture));
		}
	}
}
