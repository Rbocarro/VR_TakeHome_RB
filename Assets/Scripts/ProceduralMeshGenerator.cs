using UnityEngine;

namespace Demo.Utility
{
    [System.Serializable]
    public class ProceduralMeshGenerator
    {
        public Material defaultMeshMaterial;

        [Header("Cone Mesh Settings")]
        public int sphereMeshResolution = 32;
        [SerializeField, Range(0.1f, 5f)]
        private float sphereMeshRadius = 1;

        [Header("Sphere Mesh Settings")]
        [SerializeField]
        private int coneMeshSegments = 8;
        [SerializeField]
        private float coneMeshHeight = 0.075f;
        [SerializeField]
        private float coneMeshBaseRadius = 0.03f;
        // ref:https://danielsieger.com/blog/2021/03/27/generating-spheres.html
        Mesh GenerateUVSphereMesh(int resolution, float radius)//Generate UV sphere
        {
            //create new mesh object
            Mesh mesh = new Mesh();

            //calculate number of segments  and rings
            int segments = resolution;
            int rings = resolution / 2;

            //calculate total number of vertices
            // (segments * (rings - 1)) for the body + 2 for the top and bottom vertices
            Vector3[] vertices = new Vector3[(segments * (rings - 1)) + 2];

            //calculate the total number of triangles

            int[] triangles = new int[(segments * 6 * (rings - 1)) + (segments * 6)];

            //add top vertex to sphere
            vertices[0] = new Vector3(0, radius, 0);

            //generate vertices for each segments and rings
            int vertexIndex = 1;
            for (int i = 0; i < rings - 1; i++)
            {
                //calculate poloar angle phi
                float phi = Mathf.PI * (i + 1) / rings;
                for (int j = 0; j < segments; j++)
                {
                    //calculate azimuthal angle theta
                    float theta = 2.0f * Mathf.PI * j / segments;
                    //calculate vertex position using using spherical coordinates and scale by radius
                    float x = radius * Mathf.Sin(phi) * Mathf.Cos(theta);
                    float y = radius * Mathf.Cos(phi);
                    float z = radius * Mathf.Sin(phi) * Mathf.Sin(theta);
                    //add vertex to the vertices array
                    vertices[vertexIndex++] = new Vector3(x, y, z);
                }
            }

            //add the bottom vertex of the sphere
            vertices[vertexIndex] = new Vector3(0, -radius, 0);

            //add triangles to the bottom and top caps
            int triangleIndex = 0;
            for (int i = 0; i < segments; i++)
            {
                // Top cap triangles
                int i0 = (i + 1) % segments + 1;
                int i1 = i + 1;
                triangles[triangleIndex++] = 0;
                triangles[triangleIndex++] = i0;
                triangles[triangleIndex++] = i1;

                // Bottom cap triangles
                i0 = (i + 1) % segments + segments * (rings - 2) + 1;
                i1 = i + segments * (rings - 2) + 1;
                triangles[triangleIndex++] = vertexIndex;
                triangles[triangleIndex++] = i1;
                triangles[triangleIndex++] = i0;
            }
            // Add quads for the body of the sphere
            for (int j = 0; j < rings - 2; j++)
            {
                int j0 = j * segments + 1;
                int j1 = (j + 1) * segments + 1;
                for (int i = 0; i < segments; i++)
                {
                    int i0 = j0 + i;
                    int i1 = j0 + (i + 1) % segments;
                    int i2 = j1 + (i + 1) % segments;
                    int i3 = j1 + i;

                    // First triangle of the quad
                    triangles[triangleIndex++] = i0;
                    triangles[triangleIndex++] = i1;
                    triangles[triangleIndex++] = i2;

                    // Second triangle of the quad
                    triangles[triangleIndex++] = i0;
                    triangles[triangleIndex++] = i2;
                    triangles[triangleIndex++] = i3;
                }
            }

            // Assign the vertices and triangles to the mesh
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            // Recalculate normals for proper lighting
            mesh.RecalculateNormals();

            return mesh;
        }
        Mesh GenerateConeMesh(int coneSegments, float coneMeshHeight, float coneMeshBaseRadius)
        {
            Mesh mesh = new Mesh();
            //tip+ center of the base+base ring
            int vertexCount = coneSegments + 2;
            Vector3[] vertices = new Vector3[vertexCount];

            //assign vertex 0 to be tip of cone 
            vertices[0] = Vector3.forward * coneMeshHeight;

            //assign vertex 1 to be center of  cone vertex
            vertices[1] = Vector3.zero;

            //generate circular base vertices
            float angleStep = 2 * Mathf.PI / coneSegments;
            for (int i = 0; i < coneSegments; i++)
            {
                float angle = i * angleStep;
                float x = Mathf.Cos(angle) * coneMeshBaseRadius;
                float y = Mathf.Sin(angle) * coneMeshBaseRadius;
                vertices[i + 2] = new Vector3(x, y, 0);

            }

            //generate triangle indices
            int[] triangles = new int[coneSegments * 6]; // 3 indices * 2 triangles per segment
            int triIndex = 0;
            for (int i = 0; i < coneSegments; i++)
            {
                int current = i + 2;
                int next = ((i + 1) % coneSegments) + 2;

                // Triangle from tip to base edge
                triangles[triIndex++] = 0;
                triangles[triIndex++] = current;
                triangles[triIndex++] = next;

                // Triangle for base circle
                triangles[triIndex++] = 1;
                triangles[triIndex++] = next;
                triangles[triIndex++] = current;
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            return mesh;
        }

        public GameObject GenerateDemoGameobject(string gameObjectName)
        {
            //Create Main sphere gameobject
            GameObject sphereObject = new GameObject(gameObjectName);
            MeshRenderer meshRenderer = sphereObject.AddComponent<MeshRenderer>();
            meshRenderer.material = defaultMeshMaterial;
            MeshFilter meshFilter = sphereObject.AddComponent<MeshFilter>();
            meshFilter.mesh = GenerateUVSphereMesh(sphereMeshResolution, sphereMeshRadius);

            //Create Cone gameObject
            GameObject coneObject = new GameObject("Cone");
            coneObject.transform.SetParent(sphereObject.transform);
            MeshFilter coneFilter = coneObject.AddComponent<MeshFilter>();
            MeshRenderer coneRenderer = coneObject.AddComponent<MeshRenderer>();
            coneFilter.mesh = GenerateConeMesh(coneMeshSegments, coneMeshHeight, coneMeshBaseRadius);
            coneRenderer.material = defaultMeshMaterial;

            //move cone to edge of sphere radius
            coneObject.transform.localPosition = Vector3.forward * sphereMeshRadius;

            // sphereObject.transform.position = new Vector3(GetComponent<XROrigin>().Camera.transform.position.x, <XROrigin>().Camera.transform.position.y, GetComponent<XROrigin>().Camera.transform.position.z+5f);

            return sphereObject;

        }


    }
}


