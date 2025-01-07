namespace LottoClassLibrary
{
	public class LottoNumber
	{
		public int Value { get; private set; }
		public int Row { get; set; }
		public bool IsMarked { get; set; }

		public LottoNumber(int value)
		{
			Value = value;
		}
	}
}
