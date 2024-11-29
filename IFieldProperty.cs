using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoWinner
{
	public interface IFieldProperty
	{
		int CountOfColumns { get; }
		int CountOfNumbers { get; }
		int CountOfNumbersInField { get; }
		int ColumnsWithOneNumber { get; }
		int ColumnsWithTwoNumbers { get; }
		List<int> GenerateNumbers(List<int> existingNumbers);
	}

}
