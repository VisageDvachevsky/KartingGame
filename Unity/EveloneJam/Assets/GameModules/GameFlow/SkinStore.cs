using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.GameFlow
{
    public class SkinStore
    {
        private const string SkinsPlayerPrefs = "Skins";

        public enum KartColor {
            White,
            Black,
            Red,
            Yellow,
            Green,
            Blue,
            LightBlue,
            Purple,
            Gray,
        }

        public enum KartCharacter
        {
            Evelone,
            Buster
        }

        [System.Serializable]
        private struct StoreData
        {
            public List<KartColor> AvailableColors;
            public List<KartCharacter> AvailableCharacters;
        }

        public event Action StateUpdated;

        private List<KartColor> _availableColors;
        private List<KartCharacter> _availableCharacters;

        public SkinStore()
        {
            LoadData();
        }

        public void UnlockColor(KartColor color)
        {
            if (!_availableColors.Contains(color))
            {
                _availableColors.Add(color);
                SaveData();
                StateUpdated?.Invoke();
            }
        }

        public void UnlockCharacter(KartCharacter character)
        {
            if (!_availableCharacters.Contains(character))
            {
                _availableCharacters.Add(character);
                SaveData();
                StateUpdated?.Invoke();
            }
        }

        public bool HasColor(KartColor color)
        {
            return _availableColors.Contains(color);
        }

        public bool HasCharacter(KartCharacter character)
        {
            return _availableCharacters.Contains(character);
        }

        private void LoadData()
        {
            if (PlayerPrefs.HasKey(SkinsPlayerPrefs))
            {
                string json = PlayerPrefs.GetString(SkinsPlayerPrefs);
                StoreData data = (StoreData)JsonUtility.FromJson(json, typeof(StoreData));

                _availableColors = data.AvailableColors;
                _availableCharacters = data.AvailableCharacters;
            }
            else
            {
                _availableCharacters = new List<KartCharacter>() { KartCharacter.Evelone, KartCharacter.Buster };
                _availableColors = new List<KartColor>() { KartColor.White };
            }
        }

        private void SaveData()
        {
            StoreData data = new StoreData
            {
                AvailableColors = _availableColors,
                AvailableCharacters = _availableCharacters
            };

            string json = JsonUtility.ToJson(data);

            PlayerPrefs.SetString(SkinsPlayerPrefs, json);
            PlayerPrefs.Save();
        }
    }
}