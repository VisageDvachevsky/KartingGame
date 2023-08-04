using UnityEngine;
using Zenject;

namespace Project.GameFlow
{
    public class PauseInput : MonoBehaviour
    {
        private PauseController _pauseController;

        [Inject]
        private void Construct(PauseController pauseController)
        {
            _pauseController = pauseController;
        }

        private void Update()
        {
            if (Input.GetButtonDown("Cancel"))
            {
                if (_pauseController.Paused) _pauseController.Unpause();
                else _pauseController.Pause();
            }
        }
    }
}