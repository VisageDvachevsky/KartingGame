using DG.Tweening;
using Project.Kart;
using System.Collections;
using TMPro;
using UnityEngine;
using Zenject;

namespace Project.GameFlow
{
    public class Tutorial : MonoBehaviour
    {
        private const string TutorialPlayerPrefs = "TutorialPassed";

        [SerializeField] private TextMeshProUGUI _text;

        private PlayerKartProvider _kartProvider;

        [Inject]
        private void Construct(PlayerKartProvider kartProvider)
        {
            _kartProvider = kartProvider;
            _text.text = "";
        }

        public void StartTutorial()
        {
            if (PlayerPrefs.HasKey(TutorialPlayerPrefs))
                return;

            StartCoroutine(TutorialRoutine());
        }

        private IEnumerator TutorialRoutine()
        {
            KartController kart = _kartProvider.Kart;
            _text.text = "������� A/D, ����� ���������";
            yield return new WaitUntil(() => kart.KartInput.GetHorizontal() != 0f);

            _text.DOComplete();
            _text.DOFade(0, 0.5f);
            yield return new WaitForSeconds(0.5f);
            _text.text = "������� ����� Ctrl, ����� ����������";
            _text.DOFade(1, 0.5f);
            yield return new WaitUntil(() => kart.InBoost);

            _text.DOComplete();
            _text.DOFade(0, 0.5f);
            yield return new WaitForSeconds(0.5f);
            _text.text = "������� ������, ����� �������������, ����� ����� � �����";
            _text.DOFade(1, 0.5f);
            yield return new WaitUntil(() => kart.InDrift);

            _text.DOComplete();
            _text.DOFade(0, 0.5f);
            yield return new WaitForSeconds(0.5f);
            _text.text = "������������ ������� ��������� � ������� ������ � ����� �����";
            _text.DOFade(1, 0.5f);
            yield return new WaitForSeconds(0.5f);

            _text.DOComplete();
            _text.DOFade(0, 0.5f);
            yield return new WaitForSeconds(0.5f);
            _text.text = "����������� �������� � �������� ������ ����������";
            _text.DOFade(1, 0.5f);
            yield return new WaitForSeconds(2.5f);

            _text.DOComplete();
            _text.DOFade(0, 0.5f);
            yield return new WaitForSeconds(0.5f);
            _text.text = "�������";
            _text.DOFade(1, 0.5f);
            yield return new WaitForSeconds(2.5f);
            _text.DOFade(0, 0.5f);

            PlayerPrefs.SetInt(TutorialPlayerPrefs, 1);
            PlayerPrefs.Save();
        }
    }
}
