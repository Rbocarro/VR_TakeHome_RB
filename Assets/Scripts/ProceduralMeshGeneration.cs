using UnityEngine;

public class ProceduralMeshGeneration : MonoBehaviour
{
    private Material material;

    [Header("Cone Mesh Settings")]
    [SerializeField]
    private int sphereMeshResolution;
    [SerializeField]
    private int sphereMeshRadius = 1;

    [Header("Sphere Mesh Settings")]
    [SerializeField]
    private int coneMeshResolution;



    void Start()
    {
        GenerateSphereMesh(sphereMeshResolution,sphereMeshRadius);
        GenerateConeMesh();
    }


    void GenerateSphereMesh(int resolution,int radius)
    {
        GameObject sphereObject = new GameObject("Object A");
        MeshFilter meshFilter = sphereObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = sphereObject.AddComponent<MeshRenderer>();
        meshRenderer.material = material;

        Debug.Log("Generated sphere mesh");
    }

    void GenerateConeMesh()
    {
        Debug.Log("Generated cone mesh");
    }


}
