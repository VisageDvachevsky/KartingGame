using DG.Tweening;
using UnityEngine;

namespace Project.UI
{
    public class HUD : MonoBehaviour
    {
        private static readonly Vector3 OutScale = Vector3.one * 10f;

        [SerializeField] private Transform _container;
        [SerializeField] private float _animationTime = 0.5f;

        private void Start()
        {
            _container.localScale = OutScale;
        }

        public void Show()
        {
            _container.localScale = OutScale;
            _container.DOScale(Vector3.one, _animationTime);
        }

        public void Hide()
        {
            _container.localScale = Vector3.one;
            _container.DOScale(OutScale, _animationTime);
        }
    }
}