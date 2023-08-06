using System;

namespace Project.Interaction
{
    public class CoinSystem
    {
        public event Action<int> CoinsAmountChanged;
        public int CoinsAmount { get; private set; } = 0;

        public void AddCoin()
        {
            CoinsAmount++;
            CoinsAmountChanged?.Invoke(CoinsAmount);
        }
    }
}
