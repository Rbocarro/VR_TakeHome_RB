using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class VertexDisplacement
{
    [Header("Noise Controls")]
    [SerializeField, Range(1, 6)]
    private float noiseScale=1f;
    [SerializeField, Range(0, 6)]
    private float noiseDisplacement=1f;
    [SerializeField, Range(0, 6)]
    public float scrollSpeed = 1.0f;
    public Material vertexAnimationMaterial;

    public Slider noiseScaleSlider;
    public Slider noiseDisplacementSlider;
    public Slider scrollSpeedSlider;
}
