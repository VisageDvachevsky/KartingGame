using Project.GameFlow;
using Project.Interaction;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Project.UI
{
    public class CharacterShopItem : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private SkinStore.KartCharacter _character;
        [SerializeField] private int _itemPrice;
        [SerializeField] private Image _selection;
        [SerializeField] private Transform _priceContainer;
        [SerializeField] private TextMeshProUGUI _priceText;

        private GlobalMoney _globalMoney;
        private SkinStore _skinStore;
        private SkinSettings _skinSettings;

        [Inject]
        private void Construct(GlobalMoney globalMoney, SkinStore skinStore, SkinSettings skinSettings)
        {
            _globalMoney = globalMoney;
            _skinStore = skinStore;
            _skinSettings = skinSettings;
        }

        private void Start()
        {
            UpdateVisuals();
        }

        private void OnEnable()
        {
            _globalMoney.OnMoneyAmountChanged += UpdateVisuals;
            _skinSettings.StateUpdated += UpdateVisuals;
            _skinStore.StateUpdated += UpdateVisuals;
        }

        private void OnDisable()
        {
            _globalMoney.OnMoneyAmountChanged -= UpdateVisuals;
            _skinSettings.StateUpdated -= UpdateVisuals;
            _skinStore.StateUpdated -= UpdateVisuals;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_skinStore.HasCharacter(_character) && _globalMoney.TrySpend(_itemPrice))
                _skinStore.UnlockCharacter(_character);

            if (_skinStore.HasCharacter(_character))
                _skinSettings.SetCharacter(_character);

            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            _priceText.text = _itemPrice.ToString();
            _priceText.color = _globalMoney.MoneyAmount >= _itemPrice ? Color.white : Color.red;

            _priceContainer.gameObject.SetActive(!_skinStore.HasCharacter(_character));

            _selection.color = _skinSettings.CurrentCharacter == _character ? Color.white : Color.black;
        }
    }
}