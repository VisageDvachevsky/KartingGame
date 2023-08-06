using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.GameFlow
{
    public class SkinSettings
    {
        private const string CurrentSkinPlayerPrefs = "CurrentSkin";

        [System.Serializable]
        private struct StoreData
        {
            public SkinStore.KartColor CurrentColor;
            public SkinStore.KartCharacter CurrentCharacter;
        }

        public event Action StateUpdated;

        public SkinStore.KartColor CurrentColor { get; private set; }
        public SkinStore.KartCharacter CurrentCharacter { get; private set; }

        public SkinSettings()
        {
            LoadData();
        }

        public void SetColor(SkinStore.KartColor color)
        {
            if (CurrentColor == color)
                return;

            CurrentColor = color;
            SaveData();
            StateUpdated?.Invoke();
        }

        public void SetCharacter(SkinStore.KartCharacter character)
        {
            if (CurrentCharacter == character)
                return;

            CurrentCharacter = character;
            SaveData();
            StateUpdated?.Invoke();
        }

        private void LoadData()
        {
            if (PlayerPrefs.HasKey(CurrentSkinPlayerPrefs))
            {
                string json = PlayerPrefs.GetString(CurrentSkinPlayerPrefs);
                StoreData data = (StoreData)JsonUtility.FromJson(json, typeof(StoreData));

                CurrentColor = data.CurrentColor;
                CurrentCharacter = data.CurrentCharacter;
            }
            else
            {
                CurrentColor = SkinStore.KartColor.White;
                CurrentCharacter = SkinStore.KartCharacter.Evelone;
            }
        }

        private void SaveData()
        {
            StoreData data = new StoreData
            {
                CurrentCharacter = CurrentCharacter,
                CurrentColor = CurrentColor
            };

            string json = JsonUtility.ToJson(data);

            PlayerPrefs.SetString(CurrentSkinPlayerPrefs, json);
            PlayerPrefs.Save();
        }
    }
}