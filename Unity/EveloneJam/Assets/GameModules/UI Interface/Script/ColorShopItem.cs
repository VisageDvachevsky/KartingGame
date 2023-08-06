using Project.GameFlow;
using Project.Interaction;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Project.UI
{
    public class ColorShopItem : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private SkinStore.KartColor _color;
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
            if (!_skinStore.HasColor(_color) && _globalMoney.TrySpend(_itemPrice))
                _skinStore.UnlockColor(_color);

            if (_skinStore.HasColor(_color))
                _skinSettings.SetColor(_color);

            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            _priceText.text = _itemPrice.ToString();
            _priceText.color = _globalMoney.MoneyAmount >= _itemPrice ? Color.white : Color.red;

            _priceContainer.gameObject.SetActive(!_skinStore.HasColor(_color));

            _selection.color = _skinSettings.CurrentColor == _color ? Color.white : Color.black;
        }
    }
}