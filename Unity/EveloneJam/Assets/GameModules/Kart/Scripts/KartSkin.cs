using Project.GameFlow;
using TMPro;
using UnityEngine;

namespace Project.Kart
{
    public class KartSkin : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        [Header("Colors")]
        [SerializeField] private Material _white;
        [SerializeField] private Material _black;
        [SerializeField] private Material _red;
        [SerializeField] private Material _yellow;
        [SerializeField] private Material _green;
        [SerializeField] private Material _blue;
        [SerializeField] private Material _lightBlue;
        [SerializeField] private Material _purple;
        [SerializeField] private Material _gray;
        [Header("Characters")]
        [SerializeField] private GameObject _buster;
        [SerializeField] private GameObject _evelone;


        public void SetSkin(SkinStore.KartColor color, SkinStore.KartCharacter character)
        {
            SetColor(color);
            SetCharacter(character);
        }

        private void SetCharacter(SkinStore.KartCharacter character)
        {
            switch(character)
            {
                case SkinStore.KartCharacter.Evelone:
                    _evelone.SetActive(true);
                    _buster.SetActive(false);
                    break;
                case SkinStore.KartCharacter.Buster:
                    _evelone.SetActive(false);
                    _buster.SetActive(true);
                    break;
            }
        }

        private void SetColor(SkinStore.KartColor color)
        {
            switch (color)
            {
                case SkinStore.KartColor.White:
                    _renderer.material = _white;
                    break;
                case SkinStore.KartColor.Black:
                    _renderer.material = _black;
                    break;
                case SkinStore.KartColor.Red:
                    _renderer.material = _red;
                    break;
                case SkinStore.KartColor.Yellow:
                    _renderer.material = _yellow;
                    break;
                case SkinStore.KartColor.Green:
                    _renderer.material = _green;
                    break;
                case SkinStore.KartColor.Blue:
                    _renderer.material = _blue;
                    break;
                case SkinStore.KartColor.LightBlue:
                    _renderer.material = _lightBlue;
                    break;
                case SkinStore.KartColor.Purple:
                    _renderer.material = _purple;
                    break;
                case SkinStore.KartColor.Gray:
                    _renderer.material = _gray;
                    break;
            }
        }
    }
}