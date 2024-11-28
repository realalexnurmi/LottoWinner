using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoWinner
{
	public interface IFillingStrategy
	{
		List<int> GenerateNumbers(LottoGame game, List<int> existingNumbers);
	}
}
