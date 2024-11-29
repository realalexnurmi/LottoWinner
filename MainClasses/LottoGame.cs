﻿using System;
using System.Numerics;
using System.Collections.Generic;

namespace LottoWinner
{
	public class LottoGame
	{
		public IFieldProperty FieldProperty { get; private set; }
		private BigRational[] factorialCache;
		private List<FieldCategory> fieldCategories = new List<FieldCategory>();
		private List<LottoPlayer> players = new List<LottoPlayer>();

		public LottoGame(IFieldProperty fieldProperty)
		{
			this.FieldProperty = fieldProperty;
			this.factorialCache = new BigRational[this.FieldProperty.CountOfNumbers + 2];
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

		public void AddPlayer(LottoPlayer player)
		{
			players.Add(player);
			player.SetNumber(players.Count);
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

		public void ShowPlayers()
		{
			foreach (var player in players)
			{
				player.ShowTickets();
				Console.WriteLine(new string('-', 30));
			}
		}

		private void InitializeFieldCategories()
		{
			int columnsWithOneNumber = FieldProperty.ColumnsWithOneNumber;
			int columnsWithTwoNumbers = FieldProperty.ColumnsWithTwoNumbers;

			GenerateUniqueCategories(columnsWithOneNumber, columnsWithTwoNumbers);

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

		private void GenerateUniqueCategories(int columnsWithOneNumber, int columnsWithTwoNumbers)
		{
			var allColumns = Enumerable.Range(1, FieldProperty.CountOfColumns).ToList();
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

		public LottoTicket GenerateTicket(int first, int second)
		{
			var ticket = GenerateTicket();
			ticket.TicketNumber = first * 1000 + second;
			return ticket;
		}

		private LottoField GenerateField(List<int> existingNumbers)
		{
			return new LottoField(this, FieldProperty, existingNumbers);
		}

		public IReadOnlyList<FieldCategory> GetFieldCategories()
		{
			return fieldCategories.AsReadOnly();
		}

		public class FieldCategory
		{
			public List<int> ColumnsWithOneNumber { get; private set; }
			private LottoGame game;
			private BigRational countValidFields;
			public int Rang { get; set; }

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

				foreach (var col in ColumnsWithOneNumber)
				{
					int start = (col - 1) * 10 + (col == 1 ? 1 : 0);
					int end = (col == 1) ? 9 : (col == game.FieldProperty.CountOfColumns) ? game.FieldProperty.CountOfNumbers : start + 9;
					categoryCombinations *= game.Combinations(end - start + 1, 1);
				}

				for (int col = 1; col <= game.FieldProperty.CountOfColumns; col++)
				{
					if (!ColumnsWithOneNumber.Contains(col))
					{
						int start = (col - 1) * 10 + (col == 1 ? 1 : 0);
						int end = (col == 1) ? 9 : (col == game.FieldProperty.CountOfColumns) ? game.FieldProperty.CountOfNumbers : start + 9;
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
				Console.Write("] Valid fields: " + CountValidFields);
				Console.WriteLine();
			}
		}

public void RunSeriesInParallel(int countSeries, int countGamesPerSeries)
{
    Parallel.For(0, countSeries, seriesIndex =>
    {
        RunSeries(countGamesPerSeries);
    });
}

private void RunSeries(int countGamesPerSeries)
{
    var seriesWins = new Dictionary<int, int>();

    foreach (var player in players)
    {
        var seriesTickets = player.CloneTickets();
        seriesWins[player.PlayerNumber] = 0; // Инициализация счетчика побед для каждого игрока

        Parallel.For(0, countGamesPerSeries, gameIndex =>
        {
            int winner = StartGame(true, seriesTickets);
            lock (seriesWins)
            {
                seriesWins[winner]++;
            }
        });
    }

    lock (Console.Out)
    {
        Console.WriteLine("Series Game Statistics:");
        foreach (var kvp in seriesWins)
        {
            double winPercentage = (double)kvp.Value / countGamesPerSeries * 100;
            Console.WriteLine($"Player {kvp.Key} won {kvp.Value} times ({winPercentage:F2}%).");
        }
    }
}

		private int StartGame(bool silent, List<LottoTicket> seriesTickets)
		{
			var random = new Random();
			var allNumbers = Enumerable.Range(1, FieldProperty.CountOfNumbers).ToList();
			var drawnNumbers = new HashSet<int>();

			// Сброс состояний перед началом новой игры
			foreach (var ticket in seriesTickets)
			{
				ticket.Field1.ResetNumberStates();
				ticket.Field2.ResetNumberStates();
			}

			while (true)
			{
				if (allNumbers.Count == 0)
				{
					throw new InvalidOperationException("No more numbers to draw.");
				}

				int drawnNumber = allNumbers[random.Next(allNumbers.Count)];
				drawnNumbers.Add(drawnNumber);
				allNumbers.Remove(drawnNumber);

				if (!silent)
				{
					Console.WriteLine($"Drawn number: {drawnNumber}");
				}

				foreach (var ticket in seriesTickets)
				{
					ticket.Field1.MarkNumberAsDrawn(drawnNumber);
					if (ticket.Field1.AreAllNumbersDrawn())
					{
						if (!silent)
						{
							Console.WriteLine($"Player {ticket.TicketNumber.Value / 1000} wins with ticket {ticket.TicketNumber}!");
						}
						return ticket.TicketNumber.Value / 1000;
					}

					ticket.Field2.MarkNumberAsDrawn(drawnNumber);
					if (ticket.Field2.AreAllNumbersDrawn())
					{
						if (!silent)
						{
							Console.WriteLine($"Player {ticket.TicketNumber.Value / 1000} wins with ticket {ticket.TicketNumber}!");
						}
						return ticket.TicketNumber.Value / 1000;
					}
				}
			}
		}
	}
}
