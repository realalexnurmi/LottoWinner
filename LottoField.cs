using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoWinner
{
	public class LottoField
	{
		public LottoGame.FieldCategory Category { get; private set; }
		public List<int> Numbers { get; private set; }

		public LottoField(LottoGame game, IFillingStrategy fillingStrategy, List<int> existingNumbers)
		{
			Numbers = fillingStrategy.GenerateNumbers(game, existingNumbers);
			Category = DetermineCategory(game);
		}

		private LottoGame.FieldCategory DetermineCategory(LottoGame game)
		{
			foreach (var category in game.GetFieldCategories())
			{
				if (IsCategoryMatch(category))
				{
					return category;
				}
			}
			throw new InvalidOperationException("No matching category found for the field.");
		}

		private bool IsCategoryMatch(LottoGame.FieldCategory category)
		{
			var columnCounts = new int[category.Game.CountOfColumns];

			foreach (var number in Numbers)
			{
				int column = (number - 1) / 10 + 1;
				columnCounts[column - 1]++;
			}

			foreach (var col in category.ColumnsWithOneNumber)
			{
				if (columnCounts[col - 1] != 1)
				{
					return false;
				}
			}

			for (int col = 1; col <= category.Game.CountOfColumns; col++)
			{
				if (!category.ColumnsWithOneNumber.Contains(col) && columnCounts[col - 1] != 2)
				{
					return false;
				}
			}

			return true;
		}
	}
}
