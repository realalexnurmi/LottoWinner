using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoWinner
{
	public class DefaultRulesFieldProperty : IFieldProperty
	{
		public int CountOfColumns { get; private set; }
		public int CountOfNumbers { get; private set; }
		public int CountOfNumbersInField { get; private set; }
		public int ColumnsWithOneNumber { get; private set; }
		public int ColumnsWithTwoNumbers { get; private set; }

		public DefaultRulesFieldProperty(int countOfColumns)
		{
			if (countOfColumns < 3 || countOfColumns > 9 || countOfColumns % 3 != 0)
			{
				throw new ArgumentException("Number of columns must be between 3 and 9 and divisible by 3.");
			}
			this.CountOfColumns = countOfColumns;
			this.CountOfNumbers = countOfColumns * 10;
			this.CountOfNumbersInField = (countOfColumns / 3) * 5;

			int maxColumnsWithTwoNumbers = CountOfNumbersInField / 2;
			int columnsWithOneNumber = CountOfColumns - maxColumnsWithTwoNumbers;
			while (columnsWithOneNumber + 2 * (CountOfColumns - columnsWithOneNumber) != CountOfNumbersInField)
			{
				columnsWithOneNumber++;
				maxColumnsWithTwoNumbers--;
			}
			this.ColumnsWithOneNumber = columnsWithOneNumber;
			this.ColumnsWithTwoNumbers = maxColumnsWithTwoNumbers;
		}

		public List<int> GenerateNumbers(List<int> existingNumbers)
		{
			var random = new Random();
			var numbers = new List<int>[CountOfColumns];
			for (int i = 0; i < CountOfColumns; i++)
			{
				numbers[i] = new List<int>();
			}

			var allNumbers = Enumerable.Range(1, CountOfNumbers).ToList();
			foreach (var number in existingNumbers)
			{
				allNumbers.Remove(number);
			}

			var columnCounts = new int[CountOfColumns];

			// Заполняем столбцы с одним числом
			for (int i = 0; i < ColumnsWithOneNumber; i++)
			{
				int column = random.Next(CountOfColumns);
				while (columnCounts[column] >= 1)
				{
					column = random.Next(CountOfColumns);
				}

				var availableNumbers = allNumbers.Where(n => (n - 1) / 10 == column).ToList();
				if (availableNumbers.Count > 0)
				{
					int number = availableNumbers[random.Next(availableNumbers.Count)];
					numbers[column].Add(number);
					columnCounts[column]++;
					allNumbers.Remove(number);
				}
			}

			// Заполняем столбцы с двумя числами
			for (int i = 0; i < CountOfColumns; i++)
			{
				if (columnCounts[i] == 0)
				{
					var availableNumbers = allNumbers.Where(n => (n - 1) / 10 == i).ToList();
					if (availableNumbers.Count >= 2)
					{
						int number1 = availableNumbers[random.Next(availableNumbers.Count)];
						availableNumbers.Remove(number1);
						int number2 = availableNumbers[random.Next(availableNumbers.Count)];
						numbers[i].Add(number1);
						numbers[i].Add(number2);
						columnCounts[i] += 2;
						allNumbers.Remove(number1);
						allNumbers.Remove(number2);
					}
				}
			}

			return numbers.SelectMany(column => column).ToList();
		}
	}
}
