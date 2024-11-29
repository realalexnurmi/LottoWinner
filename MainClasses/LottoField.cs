using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoWinner
{
	public class LottoField
	{
		public LottoGame Game { get; private set; }
		public LottoGame.FieldCategory Category { get; private set; }
		public List<int> Numbers { get; private set; }
		public Dictionary<int, bool> NumberStates { get; private set; }

		public LottoField(LottoGame game, IFieldProperty fieldProperty, List<int> existingNumbers)
		{
			this.Game = game;
			Numbers = fieldProperty.GenerateNumbers(existingNumbers);
			Category = DetermineCategory(game);
			NumberStates = Numbers.ToDictionary(number => number, number => false);
		}

		public void ResetNumberStates()
		{
			foreach (var key in NumberStates.Keys)
			{
				NumberStates[key] = false;
			}
		}

		public void MarkNumberAsDrawn(int number)
		{
			if (NumberStates.ContainsKey(number))
			{
				NumberStates[number] = true;
			}
		}

		public bool AreAllNumbersDrawn()
		{
			return NumberStates.Values.All(state => state);
		}

		private LottoGame.FieldCategory DetermineCategory(LottoGame game)
		{
			var columnCounts = new int[game.FieldProperty.CountOfColumns];

			foreach (var number in Numbers)
			{
				int column = (number - 1) / 10 + 1;
				columnCounts[column - 1]++;
			}

			foreach (var category in game.GetFieldCategories())
			{
				bool match = true;
				foreach (var col in category.ColumnsWithOneNumber)
				{
					if (columnCounts[col - 1] != 1)
					{
						match = false;
						break;
					}
				}

				if (match)
				{
					for (int col = 1; col <= game.FieldProperty.CountOfColumns; col++)
					{
						if (!category.ColumnsWithOneNumber.Contains(col) && columnCounts[col - 1] != 2)
						{
							match = false;
							break;
						}
					}
				}

				if (match)
				{
					return category;
				}
			}

			throw new InvalidOperationException("No matching category found for the field.");
		}

		public void Print()
		{
			var columns = new List<int>[Game.FieldProperty.CountOfColumns];
			for (int i = 0; i < Game.FieldProperty.CountOfColumns; i++)
			{
				columns[i] = new List<int>();
			}

			foreach (var number in Numbers)
			{
				int column = (number - 1) / 10;
				columns[column].Add(number);
			}

			Console.WriteLine($"R:{Category.Rang,2}──┬──┬──┬──┬──┬──┬──┬──┐");
			Console.Write("│");
			for (int i = 0; i < columns.Length; i++)
			{
				if (columns[i].Count > 0)
				{
					Console.Write($"{columns[i][0],2}│");
				}
				else
				{
					Console.Write("  │");
				}
			}
			Console.WriteLine();
			Console.WriteLine("├──┼──┼──┼──┼──┼──┼──┼──┼──┤");
			Console.Write("│");
			for (int i = 0; i < columns.Length; i++)
			{
				if (columns[i].Count > 1)
				{
					Console.Write($"{columns[i][1],2}│");
				}
				else
				{
					Console.Write("  │");
				}
			}
			Console.WriteLine();
			Console.WriteLine("└──┴──┴──┴──┴──┴──┴──┴──┴──┘");
		}
	}
}
