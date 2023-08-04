using System.Collections.Generic;
using UnityEngine;

namespace Project.Interaction
{
    [CreateAssetMenu]
    public class ItemConfig : ScriptableObject
    {
        [System.Serializable]
        public struct ItemEntry
        {
            [field: SerializeField] public Sprite Icon { get; set; }
            [field: SerializeField] public Item Item { get; private set; }
            [field: SerializeField] public AnimationCurve ChanceCurve { get; set; }
        }

        [SerializeField] private List<ItemEntry> _items;

        public IReadOnlyList<ItemEntry> AllItems => _items;

        public Item GetRandomItem(int place, int participantsAmount)
        {
            float placePercent = (float)place / participantsAmount;

            float[] chances = new float[_items.Count];
            float chanceSum = 0;
            for (int i = 0; i < _items.Count; i++)
            {
                ItemEntry itemEntry = _items[i];

                float chance = itemEntry.ChanceCurve.Evaluate(placePercent);
                chances[i] = chance;
                chanceSum += chance;
            }

            float decision = Random.Range(0, chanceSum);
            chanceSum = 0;
            for (int i = 0; i < _items.Count; i++)
            {
                float chance = chances[i];

                chanceSum += chance;

                if (chanceSum >= decision) return _items[i].Item;
            }

            throw new System.InvalidOperationException();
        }
    }
}