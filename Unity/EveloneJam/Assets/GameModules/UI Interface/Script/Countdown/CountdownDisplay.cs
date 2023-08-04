using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Project.UI
{
    public class CountdownDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private AudioSource _audio;
        [SerializeField] private AudioClip _countClip;
        [SerializeField] private AudioClip _raceStartClip;
        [SerializeField] private int _seconds = 3;

        private Coroutine _counting;

        public bool Active { get; private set; } = false;

        private void Start()
        {
            _text.enabled = false;
        }

        public void Activate()
        {
            if (_counting == null)
            {
                _counting = StartCoroutine(CountdownAnimation());
            }
        }

        private IEnumerator CountdownAnimation()
        {
            Active = true;
            yield return new WaitForSeconds(2f);
            _text.enabled = true;
            for (int i = _seconds; i >= 1; i--)
            {
                _text.text = i.ToString();
                _audio.PlayOneShot(_countClip);

                _text.transform.localScale = Vector3.one;
                _text.transform.DOScale(Vector3.zero, 0.75f);


                yield return new WaitForSeconds(1f);
            }

            Active = false;

            _text.text = "Start";
            _audio.PlayOneShot(_raceStartClip);

            _text.transform.localScale = Vector3.one;
            _text.transform.DOScale(Vector3.zero, 0.75f);

            yield return new WaitForSeconds(1f);

            _text.enabled = false;
        }

    }
}