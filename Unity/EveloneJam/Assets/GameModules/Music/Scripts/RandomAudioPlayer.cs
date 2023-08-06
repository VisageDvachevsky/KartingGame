using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Audio
{
    public class RandomAudioPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _audio;
        [SerializeField] private AudioClip[] _clips;

        private int _previuousIndex = 0;

        private void Start()
        {
            _audio.Play();
            _audio.clip = _clips[0];
        }

        private void Update()
        {
            if (!_audio.isPlaying)
                PlayRandomMusic();
        }

        private void PlayRandomMusic()
        {
            int index = _previuousIndex;
            if (_clips.Length > 1)
                do
                    index = Random.Range(0, _clips.Length);
                while (index == _previuousIndex);

            _audio.clip = _clips[index];
            _audio.Play();
        }
    }
}