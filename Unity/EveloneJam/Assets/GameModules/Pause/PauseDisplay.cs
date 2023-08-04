using UnityEngine;
using Zenject;

namespace Project.GameFlow
{
    public class PauseDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject _pauseText;
        [SerializeField] private LoaderManager _loaderManager;
        private PauseController _pauseController;

        [Inject]
        private void Contruct(PauseController pauseController)
        {
            _pauseController = pauseController;
        }

        private void Start()
        {
            _pauseText.SetActive(false);
        }

        private void OnEnable()
        {
            _pauseController.PauseChanged += UpdateText;
        }

        private void OnDisable()
        {
            _pauseController.PauseChanged -= UpdateText;
        }

        public void GoToMenu()
        {
            _pauseController.Unpause();
            _loaderManager.Menu();
        }

        public void Close()
        {
            _pauseController.Unpause();
        }

        private void UpdateText()
        {
            _pauseText.SetActive(_pauseController.Paused);
        }
    }
}