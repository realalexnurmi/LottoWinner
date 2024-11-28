using System;
using System.Numerics;
using System.Collections.Generic;

namespace LotoWinner
{
	public class LottoGame
	{
		private int countOfColumns;
		private int countOfNumbers;
		private int countOfNumbersInField;
		private BigRational[] factorialCache;
		private List<FieldCategory> fieldCategories = new List<FieldCategory>();

		public LottoGame(int countOfColumns)
		{
			if (countOfColumns < 3 || countOfColumns > 9 || countOfColumns % 3 != 0)
			{
				throw new ArgumentException("Number of columns must be between 3 and 9 and divisible by 3.");
			}
			this.countOfColumns = countOfColumns;
			this.countOfNumbers = countOfColumns * 10;
			this.countOfNumbersInField = (countOfColumns / 3) * 5;
			this.factorialCache = new BigRational[this.countOfNumbers + 2];
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
			// Определяем количество столбцов с одним и двумя числами
			int maxColumnsWithTwoNumbers = countOfNumbersInField / 2;
			int columnsWithOneNumber = countOfColumns - maxColumnsWithTwoNumbers;

			// Проверяем, что общее количество чисел равно countOfNumbersInField
			while (columnsWithOneNumber + 2 * (countOfColumns - columnsWithOneNumber) != countOfNumbersInField)
			{
				columnsWithOneNumber++;
				maxColumnsWithTwoNumbers--;
			}

			// Вычисляем количество возможных категорий полей
			BigRational numberOfCategories = Combinations(countOfColumns, columnsWithOneNumber);

			// Создаем список структур категорий полей
			GenerateUniqueCategories(columnsWithOneNumber, (int)numberOfCategories);

			// Сортируем категории по количеству возможных полей
			fieldCategories = fieldCategories.OrderByDescending(c => c.CountValidFields).ToList();

			// Проставляем ранги
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

		private void GenerateUniqueCategories(int columnsWithOneNumber, int numberOfCategories)
		{
			var allColumns = Enumerable.Range(1, countOfColumns).ToList();
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

			// Вычисляем количество возможных полей внутри каждой категории
			foreach (var category in fieldCategories)
			{
				totalCombinations += category.CountValidFields;
			}

			return totalCombinations;
		}

		public BigRational CalculateWinningProbabilityOnFirstWinStep()
		{
			BigRational CV = CalculateTotalValidFields();
			BigRational TC = Combinations(countOfNumbers, countOfNumbersInField);

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

		private class FieldCategory
		{
			public List<int> ColumnsWithOneNumber { get; private set; }
			private LottoGame game;
			private BigRational countValidFields;

			private int rang;
			public int Rang { get => rang; set => rang = value; }
			public BigRational CountValidFields => countValidFields;


			public FieldCategory(List<int> columnsWithOneNumber, LottoGame game)
			{
				ColumnsWithOneNumber = new List<int>(columnsWithOneNumber);
				this.game = game;
				countValidFields = CalculateValidFields();
			}

			private BigRational CalculateValidFields()
			{
				BigRational categoryCombinations = 1;

				// Вычисляем количество комбинаций для столбцов с одним числом
				foreach (var col in ColumnsWithOneNumber)
				{
					int start = (col - 1) * 10 + (col == 1 ? 1 : 0);
					int end = (col == 1) ? 9 : (col == game.countOfColumns) ? game.countOfNumbers : start + 9;
					categoryCombinations *= game.Combinations(end - start + 1, 1);
				}

				// Вычисляем количество комбинаций для столбцов с двумя числами
				for (int col = 1; col <= game.countOfColumns; col++)
				{
					if (!ColumnsWithOneNumber.Contains(col))
					{
						int start = (col - 1) * 10 + (col == 1 ? 1 : 0);
						int end = (col == 1) ? 9 : (col == game.countOfColumns) ? game.countOfNumbers : start + 9;
						categoryCombinations *= game.Combinations(end - start + 1, 2);
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
				Console.Write("] Valid fileds: " + CountValidFields);
				Console.WriteLine();
			}
		}
	}
}
