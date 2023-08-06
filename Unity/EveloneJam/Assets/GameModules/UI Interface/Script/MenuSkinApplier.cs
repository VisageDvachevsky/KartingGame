using Project.GameFlow;
using Project.Kart;
using UnityEngine;
using Zenject;

namespace Project.UI
{
    public class MenuSkinApplier : MonoBehaviour
    {
        [SerializeField] private KartSkin _skin;

        private SkinSettings _skinSettings;

        [Inject]
        private void Construct(SkinSettings skinSettings)
        {
            _skinSettings = skinSettings;

            UpdateSkin();
        }

        private void OnEnable()
        {
            _skinSettings.StateUpdated += UpdateSkin;
        }

        private void OnDisable()
        {
            _skinSettings.StateUpdated -= UpdateSkin;
        }

        private void UpdateSkin()
        {
            _skin.SetSkin(_skinSettings.CurrentColor, _skinSettings.CurrentCharacter);
        }
    }
}