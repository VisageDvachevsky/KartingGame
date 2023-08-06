using System;
using UnityEngine;

namespace Project.Interaction
{
    public class GlobalMoney
    {
        private const string PLAYER_PREFS_MONEY = "Money";

        public event Action OnMoneyAmountChanged;
        public int MoneyAmount { get; private set; }

        public GlobalMoney()
        {
            LoadData();
        }

        public void AddMoney(int amount)
        {
            if (amount < 0) throw new InvalidOperationException();

            MoneyAmount += amount;
            OnMoneyAmountChanged?.Invoke();

            SaveData();
        }

        public bool TrySpend(int amount)
        {
            if (amount < 0) throw new InvalidOperationException();

            if (amount > MoneyAmount) return false;

            MoneyAmount -= amount;
            OnMoneyAmountChanged?.Invoke();

            SaveData();

            return true;
        }

        private void LoadData()
        {
            MoneyAmount = PlayerPrefs.GetInt(PLAYER_PREFS_MONEY, 0);
        }

        private void SaveData()
        {
            PlayerPrefs.SetInt(PLAYER_PREFS_MONEY, MoneyAmount);
        }
    }
}
