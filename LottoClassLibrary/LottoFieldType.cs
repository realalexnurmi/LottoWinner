using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoClassLibrary
{
	public class LottoFieldType
	{
		public int FirstColumnWithOneNumber { get; private set; }
		public int SecondColumnWithOneNumber { get; private set; }
		public LottoFieldType(int firstColumnWithOneNumber, int secondColumnWithOneNumber)
		{
			FirstColumnWithOneNumber = firstColumnWithOneNumber;
			SecondColumnWithOneNumber = secondColumnWithOneNumber;
		}
	}
}
