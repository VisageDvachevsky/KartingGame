using Project.Kart;
using Project.Laps;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Project.Interaction
{
    public class ItemBoxSystem : MonoBehaviour
    {
        private const float ItemSelectionTime = 3f;

        private ItemConfig _itemConfig;
        private ScoreSystem _scoreSystem;

        public event Action<Item> ItemActivated;
        public KartController Kart { get; set; }
        public Item CurrentItem { get; private set; } = null;
        public bool CanHaveNewItem { get; private set; } = true;
        public bool IsSelectingItem { get; private set; } = false;

        [Inject]
        private void Construct(ScoreSystem scoreSystem, ItemConfig itemConfig)
        {
            _itemConfig = itemConfig;
            _scoreSystem = scoreSystem;
        }

        public void SelectNewItem()
        {
            if (!CanHaveNewItem)
            {
                return;
            }

            StartCoroutine(ItemSelectionRoutine());
        }

        private IEnumerator ItemSelectionRoutine()
        {
            CanHaveNewItem = false;
            IsSelectingItem = true;
            yield return new WaitForSeconds(ItemSelectionTime);
            IsSelectingItem = false;

            int place = _scoreSystem.GetPlace(Kart.CheckpointCounter);
            int participantsAmount = _scoreSystem.ScoreDatas.Count;
            CurrentItem = _itemConfig.GetRandomItem(place, participantsAmount);
        }

        public void ActivateCurrentItem()
        {
            if (CurrentItem != null)
            {
                CurrentItem.Activate(Kart);

                ItemActivated?.Invoke(CurrentItem);
                CurrentItem = null;
                CanHaveNewItem = true;
            }
        }
    }
}