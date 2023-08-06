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
        public const float ItemSelectionTime = 3f;

        private DiContainer _container;
        private ItemConfig _itemConfig;
        private ScoreSystem _scoreSystem;

        public event Action<Item> ItemActivated;

        public KartController Kart { get; set; }
        public Item CurrentItem { get; private set; } = null;
        public bool CanHaveNewItem { get; private set; } = true;
        public bool IsSelectingItem { get; private set; } = false;

        [Inject]
        private void Construct(DiContainer container, ScoreSystem scoreSystem, ItemConfig itemConfig)
        {
            _container = container;
            _itemConfig = itemConfig;
            _scoreSystem = scoreSystem;
        }

        public void SelectNewItem()
        {
            if (!CanHaveNewItem || Kart.EffectHandler.CurrentEffect != null)
                return;

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
            if (CurrentItem == null)
                return;

            Item item = _container.InstantiatePrefabForComponent<Item>(CurrentItem);
            item.transform.position = transform.position;
            if (item.TryActivate(Kart))
            {
                ItemActivated?.Invoke(item);

                CurrentItem = null;
                CanHaveNewItem = true;
            }
        }
    }
}