using DG.Tweening;
using Project.Kart;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Interaction
{
    public class ItemDisplay : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private Transform _useHint;
        [SerializeField] private Image _image;
        [SerializeField] private AudioSource _audio;
        [SerializeField] private AudioClip _newItemSound;

        private PlayerKartProvider _kartProvider;
        private ItemConfig _itemConfig;
        private ItemBoxSystem _itemBoxSystem;
        private Coroutine _colorAnimationRoutine;

        [Inject]
        private void Construct(PlayerKartProvider kartProvider, ItemConfig itemConfig)
        {
            _kartProvider = kartProvider;
            _itemConfig = itemConfig;
        }

        private void Start()
        {
            _container.localScale = Vector3.zero;
        }

        private void OnEnable()
        {
            _kartProvider.KartUpdated += SyncKart;
        }

        private void OnDisable()
        {
            _kartProvider.KartUpdated -= SyncKart;
        }

        private void Update()
        {
            if (_itemBoxSystem == null)
                return;

            if (_kartProvider.Kart.CheckpointCounter.Finished)
                return;

            UpdateInfo();
        }

        private void SyncKart(KartController kart)
        {
            _itemBoxSystem = kart.ItemBoxSystem;
        }

        private void UpdateInfo()
        {
            if (_itemBoxSystem.CurrentItem == null)
            {
                _useHint.localScale = Vector3.zero;
            }

            Vector3 targetScale = (_itemBoxSystem.IsSelectingItem || _itemBoxSystem.CurrentItem != null) ? Vector3.one : Vector3.zero;
            _container.localScale = Vector3.Lerp(_container.localScale, targetScale, Time.deltaTime * 5f);

            if (_itemBoxSystem.IsSelectingItem && _colorAnimationRoutine == null)
            {
                _colorAnimationRoutine = StartCoroutine(BackgroundColorAnimation());
            }
        }

        private IEnumerator BackgroundColorAnimation()
        {
            _audio.Play();

            while (_itemBoxSystem.CurrentItem == null)
            {
                yield return new WaitForSeconds(0.1f);
                _image.sprite = _itemConfig.AllItems[Random.Range(0, _itemConfig.AllItems.Count)].Icon;
            }

            _audio.Stop();
            _audio.PlayOneShot(_newItemSound);

            _image.sprite = _itemConfig.AllItems.Where(x => x.Item == _itemBoxSystem.CurrentItem).First().Icon;
            _useHint.localScale = Vector3.zero;
            _useHint.DOScale(Vector3.one, .5f);

            _colorAnimationRoutine = null;
        }
    }
}