using UnityEngine;

namespace Demo.VertexAnimation
{
    [System.Serializable]
    public class VertexDisplacement : MonoBehaviour
    {
        [Header("Noise Controls")]
        [SerializeField, Range(0, 10f)]
        public float noiseScale = 1f;
        [SerializeField, Range(0, 10f)]
        public float noiseDisplacement = 1f;
        [SerializeField, Range(0, 3f)]
        public float scrollSpeed = 1.0f;
        public Material vertexAnimationMaterial;

        private Mesh mesh;
        private Vector3[] originalVertices;
        private Vector3[] displacedVertices;
        private Vector3[] normals;
        private float noiseOffset;
        void Start()
        {
            mesh = GetComponent<MeshFilter>().mesh;
            originalVertices = mesh.vertices;
            displacedVertices = new Vector3[originalVertices.Length];
            normals = mesh.normals;
        }
        void Update()
        {
            noiseOffset += Time.deltaTime * scrollSpeed;

            for (int i = 0; i < originalVertices.Length; i++)//loop through each vertex.
            {
                Vector3 worldPos = transform.TransformPoint(originalVertices[i]);                                               //convert vert position to world space
                float noise = Mathf.PerlinNoise(worldPos.x * noiseScale + noiseOffset, worldPos.y * noiseScale + noiseOffset);  //generate perlin noise value
                displacedVertices[i] = originalVertices[i] + normals[i] * (noise * noiseDisplacement);                          //displace vertex based on the noise value
            }
            mesh.vertices = displacedVertices;  //assign  displaced vertices back to the mesh
            mesh.RecalculateNormals();          //recalc normals
            mesh.RecalculateBounds();           //recalc bounds
        }
        private void OnDisable()
        {
            // Revert  mesh to its original vertices when the component is disabled.
            mesh.vertices = originalVertices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }
    }

}
