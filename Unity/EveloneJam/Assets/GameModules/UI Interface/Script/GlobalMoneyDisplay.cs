using DG.Tweening;
using Project.Interaction;
using TMPro;
using UnityEngine;
using Zenject;

namespace Project.UI
{
    public class GlobalMoneyDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        private GlobalMoney _globalMoney;

        [Inject]
        private void Construct(GlobalMoney globalMoney)
        {
            _globalMoney = globalMoney;
            UpdateData();
        }

        private void OnEnable()
        {
            _globalMoney.OnMoneyAmountChanged += UpdateData;
        }

        private void OnDisable()
        {
            _globalMoney.OnMoneyAmountChanged -= UpdateData;
        }

        private void UpdateData()
        {
            string newValue = _globalMoney.MoneyAmount.ToString();

            if (newValue == _text.text)
                return;

            _text.transform.DOComplete();
            _text.transform.localScale = Vector3.one;
            _text.transform.DOPunchScale(Vector3.one * 1.1f, .2f, 1, 1);
            _text.text = newValue;
        }
    }
}