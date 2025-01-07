using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoClassLibrary
{
	public class LottoField
	{
		public LottoFieldType FieldType { get; private set; }
		public LottoColumn[] Columns { get; } = new LottoColumn[9];

		public LottoField(LottoFieldType fieldType)
		{
			FieldType = fieldType;
		}

		public LottoColumn GetColumnByNumber(int columnNumber)
		{
			return Columns[columnNumber - 1];
		}

		public DataTable ToDataTable()
		{
			DataTable table = new DataTable();
			table.Columns.Add("ColNumber", typeof(int));
			table.Columns.Add("Num1", typeof(int));
			table.Columns.Add("PosNum1", typeof(int));
			table.Columns.Add("Num2", typeof(int));
			table.Columns.Add("PosNum2", typeof(int));

			foreach (var column in Columns)
			{
				var row = table.NewRow();
				row["ColNumber"] = column.ColumnNumber;
				row["Num1"] = column.NumberOne.Value;
				row["PosNum1"] = column.NumberOne.Row;
				if (column.NumberTwo != null)
				{
					row["Num2"] = column.NumberTwo.Value;
					row["PosNum2"] = column.NumberTwo.Row;
				}
				table.Rows.Add(row);
			}

			return table;
		}
	}
}
