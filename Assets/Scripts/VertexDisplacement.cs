using UnityEngine;

public class VertexDisplacement : MonoBehaviour
{
    [Header("Noise Controls")]
    [SerializeField, Range(1, 6)]
    private float noiseScale=1f;
    [SerializeField, Range(1, 6)]
    private float noiseDisplacement=1f;
    [SerializeField]
    private Vector2 noiseDirection;


    //mesh reference
    private Mesh mesh;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
