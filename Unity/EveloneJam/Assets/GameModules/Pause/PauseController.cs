using System;
using UnityEngine;

namespace Project.GameFlow
{
    public class PauseController
    {
        public event Action PauseChanged;
        public bool Paused { get; private set; } = false;

        public void Pause()
        {
            Paused = true;
            Time.timeScale = 0;
            PauseChanged?.Invoke();
        }

        public void Unpause()
        {
            Paused = false;
            Time.timeScale = 1;
            PauseChanged?.Invoke();
        }
    }
}
