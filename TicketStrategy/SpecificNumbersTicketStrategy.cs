using LottoWinner.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoWinner
{
    public class SpecificNumbersTicketStrategy : IInitializableTicketStrategy
    {
        private List<int> requiredNumbers;
        public string StrategyName => "Specific Numbers";
        private Func<List<int>> randomLovedNumbersGenerator;
        private Func<int, int, List<int>> getInvalidNumbers;
        private Func<int> getPlayerNumber;

        public SpecificNumbersTicketStrategy(
            Func<List<int>> randomLovedNumbersGenerator,
            Func<int, int, List<int>> getInvalidNumbers,
            Func<int> getPlayerNumber)
        {
            this.requiredNumbers = new List<int>();
            this.randomLovedNumbersGenerator = randomLovedNumbersGenerator;
            this.getInvalidNumbers = getInvalidNumbers;
            this.getPlayerNumber = getPlayerNumber;
        }

        public SpecificNumbersTicketStrategy(
            List<int> requiredNumbers,
            Func<List<int>> randomLovedNumbersGenerator,
            Func<int, int, List<int>> getInvalidNumbers,
            Func<int> getPlayerNumber)
        {
            this.requiredNumbers = requiredNumbers.Distinct().ToList();
            this.randomLovedNumbersGenerator = randomLovedNumbersGenerator;
            this.getInvalidNumbers = getInvalidNumbers;
            this.getPlayerNumber = getPlayerNumber;
        }

        public bool IsRightTicket(LottoTicket ticket)
        {
            var allNumbers = ticket.Field1.Numbers.Concat(ticket.Field2.Numbers).ToList();
            return requiredNumbers.All(number => allNumbers.Contains(number));
        }

        public void Forgot(int number)
        {
            requiredNumbers.Remove(number);
        }

        public bool IsEmpty()
        {
            return requiredNumbers.Count == 0;
        }

        public void SetNumbers(List<int> numbers)
        {
            requiredNumbers = numbers.Distinct().ToList();
        }

        public List<int> GetInvalidNumbers(int start, int end)
        {
            return requiredNumbers.Where(number => number < start || number > end).ToList();
        }

        public override string ToString()
        {
            return string.Join(", ", requiredNumbers);
        }

        public void Initialize()
        {
            var playerNumber = getPlayerNumber();
            var invalidNumbers = getInvalidNumbers(1, 90); // Здесь нужно использовать данные из игры

            foreach (var number in invalidNumbers)
            {
                Forgot(number);
                Console.WriteLine($"Player № {playerNumber} forgot {number} in this game.");
            }

            if (IsEmpty())
            {
                SetNumbers(randomLovedNumbersGenerator());
            }
        }
    }
}
