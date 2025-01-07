using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoClassLibrary
{
	public class LottoSearchData
	{
		public List<LottoFieldType> SearchedFieldTypes { get; } = new List<LottoFieldType>();

		public List<LottoColumn> SearchedColumns { get; } = new List<LottoColumn> { };

		public bool IsValid()
		{
			// Проверка наличия как минимум 2 типов полей
			if (SearchedFieldTypes.Count < 2)
			{
				return false;
			}

			// Проверка наличия как минимум 2 колонок каждого значения ColumnNumber от 1 до 9
			for (int i = 1; i <= 9; i++)
			{
				if (SearchedColumns.Count(col => col.ColumnNumber == i) < 2)
				{
					return false;
				}
			}

			return true;
		}

		public LottoTicket GenerateRandomTicket()
		{
			// Выбор двух случайных типов полей
			var random = new Random();
			var fieldType1 = SearchedFieldTypes[random.Next(SearchedFieldTypes.Count)];
			var fieldType2 = SearchedFieldTypes[random.Next(SearchedFieldTypes.Count)];

			// Создание первого поля
			var field1 = new LottoField(fieldType1);
			var field2 = new LottoField(fieldType2);

			// Формирование первого поля
			AddRandomColumnsToField(field1);

			// Исключение колонок, которые могут конфликтовать с первым полем
			var columnsToExclude = SearchedColumns
				.Where(col => field1.Columns.Any(fCol => fCol.ColumnNumber == col.ColumnNumber &&
														(	(fCol.NumberOne.Value == col.NumberOne.Value) ||
															(fCol.NumberTwo != null && fCol.NumberTwo.Value == col.NumberOne.Value) ||
															(col.NumberTwo != null && col.NumberTwo.Value == fCol.NumberOne.Value) ||
															(fCol.NumberTwo != null && col.NumberTwo != null && fCol.NumberTwo.Value == col.NumberTwo.Value)
														)
												)
						)
				.ToList();
			foreach (var col in columnsToExclude)
			{
				SearchedColumns.Remove(col);
			}

			// Формирование второго поля
			AddRandomColumnsToField(field2);

			// Генерация номера билета
			string ticketNumber = GenerateTicketNumber();

			// Создание билета
			var ticket = new LottoTicket(ticketNumber, field1, field2);
			return ticket;
		}

		private void AddRandomColumnsToField(LottoField field)
		{
			var random = new Random();
			var fieldType = field.FieldType;

			// Создаем список доступных колонок для каждого номера колонки
			var availableColumns = Enumerable.Range(1, 9)
				.Select(i => SearchedColumns
					.Where(col => col.ColumnNumber == i)
					.ToList())
				.ToArray();

			// Добавляем колонки для первой строки (ColumnNumber = 1)
			var column1 = availableColumns[0][random.Next(availableColumns[0].Count)];
			field.Columns[0] = column1;
			SearchedColumns.Remove(column1);

			// Добавляем колонки для последней строки (ColumnNumber = 9)
			var column9 = availableColumns[8][random.Next(availableColumns[8].Count)];
			field.Columns[8] = column9;
			SearchedColumns.Remove(column9);

			// Добавляем колонки для средних строк (ColumnNumber от 2 до 8)
			for (int i = 1; i <= 7; i++)
			{
				if (i + 1 == fieldType.FirstColumnWithOneNumber || i + 1 == fieldType.SecondColumnWithOneNumber)
				{
					// Выбираем случайную колонку с одним числом
					var singleNumberColumns = availableColumns[i].Where(col => col.NumberTwo == null).ToList();
					var singleNumberColumn = singleNumberColumns[random.Next(singleNumberColumns.Count)];
					field.Columns[i] = singleNumberColumn;
					SearchedColumns.Remove(singleNumberColumn);
				}
				else
				{
					// Выбираем случайную колонку с двумя числами
					var doubleNumberColumns = availableColumns[i].Where(col => col.NumberTwo != null).ToList();
					var doubleNumberColumn = doubleNumberColumns[random.Next(doubleNumberColumns.Count)];
					field.Columns[i] = doubleNumberColumn;
					SearchedColumns.Remove(doubleNumberColumn);
				}
			}

			// Установка значений Row для LottoNumber
			SetRowValues(field);
		}

		private void SetRowValues(LottoField field)
		{
			// Создаем массив для хранения строк
			int[] rows = { 1, 2, 3 };

			// Для каждой колонки в поле
			for (int i = 0; i < 9; i++)
			{
				var column = field.Columns[i];

				// Если в колонке одно число
				if (column.NumberTwo == null)
				{
					// Выбираем случайную строку из доступных
					int randomIndex = new Random().Next(rows.Length);
					int selectedRow = rows[randomIndex];

					// Устанавливаем строку для числа и удаляем использованную строку из массива
					column.NumberOne.Row = selectedRow;
					rows = rows.Where((r, index) => index != randomIndex).ToArray();
				}
				else
				{
					// Если в колонке два числа, выбираем две разные строки
					int randomIndex1 = new Random().Next(rows.Length);
					int selectedRow1 = rows[randomIndex1];
					int randomIndex2 = new Random().Next(rows.Length - 1);
					if (randomIndex2 >= randomIndex1) randomIndex2++;
					int selectedRow2 = rows[randomIndex2];

					// Устанавливаем строки для чисел и удаляем использованные строки из массива
					column.NumberOne.Row = selectedRow1;
					column.NumberTwo.Row = selectedRow2;
					rows = rows.Where((r, index) => index != randomIndex1 && index != randomIndex2).ToArray();
				}

				// Восстанавливаем массив строк для следующей колонки
				rows = [1, 2, 3];
			}
		}

		private string GenerateTicketNumber()
		{
			var random = new Random();
			const string chars = "0123456789";
			return new string(Enumerable.Repeat(chars, 12).Select(s => s[random.Next(s.Length)]).ToArray());
		}
	}
}
