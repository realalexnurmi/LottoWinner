using System;
using System.Collections.Generic;
using System.Linq;

namespace LotoWinner
{
	/*	public class LottoTicket
		{
			public int[,] Field1 { get; private set; }
			public int[,] Field2 { get; private set; }

			public LottoTicket()
			{
				Field1 = new int[3, 9];
				Field2 = new int[3, 9];
				GenerateTicket();
			}

			private void GenerateTicket()
			{
				var random = new Random();
				var numbers = Enumerable.Range(1, 90).ToList();

				for (int field = 0; field < 2; field++)
				{
					int[,] currentField = field == 0 ? Field1 : Field2;
					var fieldNumbers = new List<int>();

					for (int col = 0; col < 9; col++)
					{
						int start = col * 10 + 1;
						int end = start + 9;
						var columnNumbers = numbers.Where(n => n >= start && n <= end).ToList();

						int count = random.Next(1, 3); // 1 или 2 числа в столбце
						for (int i = 0; i < count; i++)
						{
							int index = random.Next(columnNumbers.Count);
							fieldNumbers.Add(columnNumbers[index]);
							columnNumbers.RemoveAt(index);
						}
					}

					// Убедимся, что в поле ровно 15 чисел
					while (fieldNumbers.Count < 15)
					{
						int index = random.Next(numbers.Count);
						fieldNumbers.Add(numbers[index]);
						numbers.RemoveAt(index);
					}

					// Разместим числа в поле
					foreach (var number in fieldNumbers)
					{
						int col = (number - 1) / 10;
						for (int row = 0; row < 3; row++)
						{
							if (currentField[row, col] == 0)
							{
								currentField[row, col] = number;
								break;
							}
						}
					}
				}
			}

			public void PrintTicket()
			{
				Console.WriteLine("Field 1:");
				PrintField(Field1);
				Console.WriteLine("Field 2:");
				PrintField(Field2);
			}

			private void PrintField(int[,] field)
			{
				for (int row = 0; row < 3; row++)
				{
					for (int col = 0; col < 9; col++)
					{
						Console.Write(field[row, col] + "\t");
					}
					Console.WriteLine();
				}
			}
		}*/
}
