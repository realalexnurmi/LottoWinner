using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoClassLibrary
{
	public class LottoColumn
	{
		public int ColumnNumber { get; private set; }
		public LottoNumber NumberOne { get; private set; }
		public LottoNumber? NumberTwo { get; private set; }
		public LottoColumn(int columnNumber, LottoNumber numberOne, LottoNumber? numberTwo)
		{
			ColumnNumber = columnNumber;
			NumberOne = numberOne;
			NumberTwo = numberTwo;
		}
	}
}
