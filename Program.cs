using System.Globalization;
using System.Numerics;

namespace LottoWinner
{
	internal class Program
	{
		static void Main(string[] args)
		{
			// Создаем игру с определенными свойствами поля
			IFieldProperty fieldProperty = new DefaultRulesFieldProperty(9);
			LottoGame game = new LottoGame(fieldProperty);

			// Создаем одного игрока с MinRankTicketStrategy и задаем ему 30 билетов
			ITicketStrategy minRankStrategy = new MinRankTicketStrategy(1);
			LottoPlayer minRankPlayer = new LottoPlayer(game, minRankStrategy).SetDesiredTicketCount(30);

			// Создаем одного игрока с AnyTicketStrategy и копируем его 100 тысяч раз
			ITicketStrategy anyTicketStrategy = new AnyTicketStrategy();
			LottoPlayer anyTicketPlayer = new LottoPlayer(game, anyTicketStrategy);

			List<LottoPlayer> players = new List<LottoPlayer> { minRankPlayer };

			for (int i = 0; i < 10; i++)
			{
				LottoPlayer clonedPlayer = anyTicketPlayer.ClonePlayer();
				int desiredTicketCount = GetDesiredTicketCount();
				clonedPlayer.SetDesiredTicketCount(desiredTicketCount);
				players.Add(clonedPlayer);
			}

			// Генерируем билеты для всех игроков
			foreach (var player in players)
			{
				player.GetAllTickets();
			}

			// Запускаем 100 серий со 100 играми
			game.RunSeriesInParallel(10, 10);
		}

		static int GetDesiredTicketCount()
		{
			var random = new Random();
			double lambda = 0.1; // Параметр экспоненциального распределения
			double u = random.NextDouble();
			int ticketCount = (int)(-Math.Log(1 - u) / lambda) + 1; // Экспоненциальное распределение
			return Math.Min(ticketCount, 50); // Ограничиваем количество билетов до 50
		}
	}
}
