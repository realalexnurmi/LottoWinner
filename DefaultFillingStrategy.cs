using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoWinner
{
	public class DefaultFillingStrategy : IFillingStrategy
	{
		public List<int> GenerateNumbers(LottoGame game, List<int> existingNumbers)
		{
			var random = new Random();
			var numbers = new List<int>();
			var allNumbers = Enumerable.Range(1, game.CountOfNumbers).ToList();

			foreach (var number in existingNumbers)
			{
				allNumbers.Remove(number);
			}

			while (numbers.Count < game.CountOfNumbersInField)
			{
				int number = allNumbers[random.Next(allNumbers.Count)];
				numbers.Add(number);
				allNumbers.Remove(number);
			}

			return numbers;
		}
	}
}
