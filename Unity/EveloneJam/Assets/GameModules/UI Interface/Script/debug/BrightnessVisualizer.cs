using UnityEngine;

public class BrightnessVisualizer : MonoBehaviour
{
    private Material material;
    private Color baseColor;

    private void Start()
    {
        // ѕолучаем материал объекта
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            material = renderer.material;
            baseColor = material.color;
        }
    }

    public void UpdateBrightnessVisual(float brightnessValue)
    {
        // ѕри изменении €ркости мен€ем €ркость объекта, использу€ его цвет
        float brightnessFactor = 0.5f + brightnessValue * 0.5f; // ѕреобразуем значение €ркости от [0, 1] в [0.5, 1]
        material.color = baseColor * brightnessFactor;
    }
}
