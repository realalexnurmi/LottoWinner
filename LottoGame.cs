using System;
using System.Numerics;
using System.Collections.Generic;

namespace LottoWinner
{
	public class LottoGame
	{
		public int CountOfColumns { get; private set; }
		public int CountOfNumbers { get; private set; }
		public int CountOfNumbersInField { get; private set; }
		private BigRational[] factorialCache;
		private List<FieldCategory> fieldCategories = new List<FieldCategory>();
		private IFillingStrategy fillingStrategy;

		public LottoGame(int countOfColumns, IFillingStrategy fillingStrategy)
		{
			if (countOfColumns < 3 || countOfColumns > 9 || countOfColumns % 3 != 0)
			{
				throw new ArgumentException("Number of columns must be between 3 and 9 and divisible by 3.");
			}
			this.CountOfColumns = countOfColumns;
			this.CountOfNumbers = countOfColumns * 10;
			this.CountOfNumbersInField = (countOfColumns / 3) * 5;
			this.factorialCache = new BigRational[this.CountOfNumbers + 2];
			this.fillingStrategy = fillingStrategy;
			InitializeFactorialCache();
			InitializeFieldCategories();
		}

		private void InitializeFactorialCache()
		{
			factorialCache[0] = 1;
			for (int i = 1; i < factorialCache.Length; i++)
			{
				factorialCache[i] = i * factorialCache[i - 1];
			}
		}

		private BigRational Factorial(int n)
		{
			return factorialCache[n];
		}

		public BigRational Combinations(int n, int k)
		{
			if (k == 0 || k == n)
			{
				return 1;
			}
			if (k > n)
			{
				throw new InvalidOperationException("k cannot be greater than n");
			}

			BigRational result = Factorial(n) / (Factorial(k) * Factorial(n - k));

			return result;
		}

		private void InitializeFieldCategories()
		{
			int maxColumnsWithTwoNumbers = CountOfNumbersInField / 2;
			int columnsWithOneNumber = CountOfColumns - maxColumnsWithTwoNumbers;

			while (columnsWithOneNumber + 2 * (CountOfColumns - columnsWithOneNumber) != CountOfNumbersInField)
			{
				columnsWithOneNumber++;
				maxColumnsWithTwoNumbers--;
			}

			BigRational numberOfCategories = Combinations(CountOfColumns, columnsWithOneNumber);

			GenerateUniqueCategories(columnsWithOneNumber, (int)numberOfCategories);

			fieldCategories = fieldCategories.OrderByDescending(c => c.CountValidFields).ToList();

			int currentRank = 1;
			fieldCategories[0].Rang = currentRank;
			for (int i = 1; i < fieldCategories.Count; i++)
			{
				if (fieldCategories[i].CountValidFields < fieldCategories[i - 1].CountValidFields)
				{
					currentRank++;
				}
				fieldCategories[i].Rang = currentRank;
			}
		}

		public IReadOnlyList<FieldCategory> GetFieldCategories()
		{
			return fieldCategories.AsReadOnly();
		}

		private void GenerateUniqueCategories(int columnsWithOneNumber, int numberOfCategories)
		{
			var allColumns = Enumerable.Range(1, CountOfColumns).ToList();
			var combinations = new List<List<int>>();

			GenerateCombinations(allColumns, columnsWithOneNumber, 0, new List<int>(), combinations);

			foreach (var combination in combinations)
			{
				fieldCategories.Add(new FieldCategory(combination, this));
			}
		}

		private void GenerateCombinations(List<int> allColumns, int columnsWithOneNumber, int start, List<int> current, List<List<int>> combinations)
		{
			if (current.Count == columnsWithOneNumber)
			{
				combinations.Add(new List<int>(current));
				return;
			}

			for (int i = start; i < allColumns.Count; i++)
			{
				current.Add(allColumns[i]);
				GenerateCombinations(allColumns, columnsWithOneNumber, i + 1, current, combinations);
				current.RemoveAt(current.Count - 1);
			}
		}

		public BigRational CalculateTotalValidFields()
		{
			BigRational totalCombinations = 0;

			foreach (var category in fieldCategories)
			{
				totalCombinations += category.CountValidFields;
			}

			return totalCombinations;
		}

		public BigRational CalculateWinningProbabilityOnFirstWinStep()
		{
			BigRational CV = CalculateTotalValidFields();
			BigRational TC = Combinations(CountOfNumbers, CountOfNumbersInField);

			return CV / TC;
		}

		public void PrintCategories()
		{
			int i = 1;
			foreach (var category in fieldCategories)
			{
				category.PrintCategory(i);
				++i;
			}
		}

		public LottoTicket GenerateTicket()
		{
			var existingNumbers = new List<int>();
			LottoField field1 = GenerateField(existingNumbers);
			LottoField field2 = GenerateField(existingNumbers.Concat(field1.Numbers).ToList());
			return new LottoTicket(field1, field2);
		}

		private LottoField GenerateField(List<int> existingNumbers)
		{
			return new LottoField(this, fillingStrategy, existingNumbers);
		}

		public class FieldCategory
		{
			public List<int> ColumnsWithOneNumber { get; private set; }
			public LottoGame Game { get; private set; }
			private BigRational countValidFields;
			public int Rang { get; set; }

			public BigRational CountValidFields => countValidFields;

			public FieldCategory(List<int> columnsWithOneNumber, LottoGame game)
			{
				ColumnsWithOneNumber = new List<int>(columnsWithOneNumber);
				this.Game = game;
				countValidFields = CalculateValidFields();
			}

			private BigRational CalculateValidFields()
			{
				BigRational categoryCombinations = 1;

				foreach (var col in ColumnsWithOneNumber)
				{
					int start = (col - 1) * 10 + (col == 1 ? 1 : 0);
					int end = (col == 1) ? 9 : (col == Game.CountOfColumns) ? Game.CountOfNumbers : start + 9;
					categoryCombinations *= Game.Combinations(end - start + 1, 1);
				}

				for (int col = 1; col <= Game.CountOfColumns; col++)
				{
					if (!ColumnsWithOneNumber.Contains(col))
					{
						int start = (col - 1) * 10 + (col == 1 ? 1 : 0);
						int end = (col == 1) ? 9 : (col == Game.CountOfColumns) ? Game.CountOfNumbers : start + 9;
						categoryCombinations *= Game.Combinations(end - start + 1, 2);
					}
				}

				return categoryCombinations;
			}

			public void PrintCategory(int i)
			{
				Console.Write($"{i} category with columns with one number [ ");
				foreach (var col in ColumnsWithOneNumber)
				{
					Console.Write(col + " ");
				}
				Console.Write("] Valid fields: " + CountValidFields);
				Console.WriteLine();
			}
		}
	}
}
