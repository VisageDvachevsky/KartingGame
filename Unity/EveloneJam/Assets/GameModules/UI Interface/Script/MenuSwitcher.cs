using UnityEngine;

namespace Project.UI
{
    public class MenuSwitcher : MonoBehaviour
    {
        [SerializeField] private GameObject _mainMenu;
        [SerializeField] private GameObject _shopMenu;

        private void Start()
        {
            _mainMenu.SetActive(true);
            _shopMenu.SetActive(false);
        }

        public void ShowMainMenu()
        {
            _mainMenu.SetActive(true);
            _shopMenu.SetActive(false);
        }

        public void ShowShopMenu()
        {
            _mainMenu.SetActive(false);
            _shopMenu.SetActive(true);
        }
    }
}