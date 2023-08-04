using UnityEngine;

public class BrightnessVisualizer : MonoBehaviour
{
    private Material material;
    private Color baseColor;

    private void Start()
    {
        // �������� �������� �������
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            material = renderer.material;
            baseColor = material.color;
        }
    }

    public void UpdateBrightnessVisual(float brightnessValue)
    {
        // ��� ��������� ������� ������ ������� �������, ��������� ��� ����
        float brightnessFactor = 0.5f + brightnessValue * 0.5f; // ����������� �������� ������� �� [0, 1] � [0.5, 1]
        material.color = baseColor * brightnessFactor;
    }
}
