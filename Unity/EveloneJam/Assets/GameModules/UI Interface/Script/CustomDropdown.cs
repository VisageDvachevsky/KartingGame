using TMPro;
using UnityEngine;

public class CustomDropdown : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public GameObject[] _Sections;

    private void Start()
    {
        if (_Sections.Length > 0)
        {
            OnDropdownValueChanged(0);
        }
        else
        {
            Debug.LogWarning("No sections defined in CustomDropdown.");
        }

        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    private void OnDropdownValueChanged(int index)
    {
        if (_Sections.Length == 0)
        {
            return;
        }

        for (int i = 0; i < _Sections.Length; i++)
        {
            _Sections[i].SetActive(false);
        }

        if (index >= 0 && index < _Sections.Length)
        {
            _Sections[index].SetActive(true);
        }
    }
}
