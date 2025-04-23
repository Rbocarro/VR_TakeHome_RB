using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using Demo.Utility;
using Demo.VertexAnimation;
using Demo.LissajousAnimation;
public class DemoManager : MonoBehaviour
{
    [Header("Demo GameObjects")]
    public GameObject objectA;
    public GameObject objectB;

    [Header("Procedural Mesh Settings")]
    public ProceduralMeshGenerator proceduralMeshGenerator;

    [Header("Lissajous Demo Settings")]
    public GameObject LissajousPanel;
    public Material lissajousTrailMaterial;//default placeholder trail material for LJ demo
    public LissajousUIBindings objectAUI;
    public LissajousUIBindings objectBUI;

    [Header("Object Rotation Demo Settings")]
    public GameObject objectRotationPanel;
    public float angularSpeed;// Object A angular Rotation Speed
    public float radiusAroundObjectA;//radius around object A where obj B spawns around
    public Slider angularSpeedSlider;
    private Coroutine rotationRoutine;

    [Header("Color Change Demo Settings")]
    public GameObject colorChangePanel;
    public float rotationSpeed;             //rotation speed of obj B around A
    public Slider rotationSpeedSlider;
    public Material colorChangeMaterial;
    private Coroutine colorChangeRoutine;

    [Header("Vertex Animation Demo Settings")]
    public GameObject vertexAnimationPanel;
    public Slider noiseScrollSpeedSlider;
    public Slider noiseScaleSlider;
    public Slider noiseDisplacementSlider;
    public Material VertexAnimationMaterial;
    private Coroutine vertexDisplacementRoutine;

    [Header("VR Attractor Settings")]
    public GameObject vrAttractorSidePanel;
    public Transform leftController;
    public Transform rightController;
    public float rightControllerattractionForce;
    public float leftControllerattractionForce;
    public Slider rightControllerattractionForceSlider;
    public Slider leftControllerattractionForceSlider;
    private Coroutine vrAttractionRoutine;

    private void Start()
    {
        
        ToggleAllPanels(false);//set all side panels active status to false at start
    }
    public void SetupLissajousDemo()
    {
        // Deactivate other panels
        TogglePanels(LissajousPanel);

        if (objectA == null) {  SpawnObjectA(); }
        if (objectB == null) {  SpawnObjectB(); }

        var xrOrigin = GetComponent<XROrigin>();
        objectA.transform.position = xrOrigin.Camera.transform.position + new Vector3(-0.5f, 0f, 1f);
        objectB.transform.position = xrOrigin.Camera.transform.position + new Vector3(0.5f, 0f, 1f);

        // Stop other coroutines
        StopAllCoroutines();

        DisableAutoRotation(objectA);
        DisableAutoRotation(objectB);

        if (objectA.GetComponent<VertexDisplacement>() != null)
        {
            objectA.GetComponent<VertexDisplacement>().enabled = false;
        }
        

        objectA.GetComponent<LissajousMovement>().enabled=true;
        objectB.GetComponent<LissajousMovement>().enabled = true;
        var lissajousA = objectA.GetComponent<LissajousMovement>();
        var lissajousB = objectB.GetComponent<LissajousMovement>();
        objectAUI.BindTo(lissajousA);
        objectBUI.BindTo(lissajousB);
    }
    public void SetupObjectRotationDemo()
    {
        if (objectA == null) { SpawnObjectA(); }
        if (objectB == null) { SpawnObjectB(); }
        // Deactivate other panels
        TogglePanels(objectRotationPanel);

        // Stop other routines
        StopAllCoroutines();
        
        DisableAutoRotation(objectA);
        DisableAutoRotation(objectB);
        objectA.GetComponent<TrailRenderer>().enabled = false;
        objectB.GetComponent<TrailRenderer>().enabled= false;
        objectA.GetComponent<LissajousMovement>().enabled = false;
        objectB.GetComponent<LissajousMovement>().enabled = false;

        if (objectA.GetComponent<VertexDisplacement>() != null)
        {
            objectA.GetComponent<VertexDisplacement>().enabled = false;
        }
        objectA.GetComponent<Renderer>().material = proceduralMeshGenerator.defaultMeshMaterial;
        objectA.GetComponent<Renderer>().material.color=Color.red;
        objectB.GetComponent<Renderer>().material = proceduralMeshGenerator.defaultMeshMaterial;
        objectB.GetComponent<Renderer>().material.color = Color.blue;

        angularSpeedSlider.onValueChanged.AddListener((value) => angularSpeed = value);
        angularSpeedSlider.value=angularSpeed;
        

        // Move objectA to be directly in front of the camera
        var xrOrigin = GetComponent<XROrigin>();
        var cameraTransform = xrOrigin.Camera.transform;
        objectA.transform.position = cameraTransform.position + cameraTransform.forward * 1f;

        // Start coroutine
        rotationRoutine = StartCoroutine(RotateTowardsMovingTarget());
    }
    private IEnumerator<WaitForEndOfFrame> RotateTowardsMovingTarget()
    {
        while (true)
        {
            // Position objectB randomly around objectA
            Vector3 randomOffset = Random.onUnitSphere;
            randomOffset = randomOffset.normalized * radiusAroundObjectA;
            objectB.transform.position = objectA.transform.position + randomOffset;

            // Rotate object A to face object B over time
            while (true)
            {
                // Calculate dir vector from rotatingObject to targetObject
                Vector3 direction = objectB.transform.position - objectA.transform.position;

                Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
                objectA.transform.rotation = Quaternion.RotateTowards(
                    objectA.transform.rotation,
                    targetRotation,
                    angularSpeed * Time.deltaTime
                );

                float angleDiff = Quaternion.Angle(objectA.transform.rotation, targetRotation);
                if (angleDiff < 1f) break;

                yield return null;
            }

        }
    }
    public void SetupColorChangeDemo()
    {
        if (objectA == null) { SpawnObjectA(); }
        if (objectB == null) { SpawnObjectB(); }
        // Deactivate other panels
        TogglePanels(colorChangePanel);

        // Stop other routines
        StopAllCoroutines();

        DisableAutoRotation(objectA);
        DisableAutoRotation(objectB);
        objectA.GetComponent<LissajousMovement>().enabled = false;
        objectB.GetComponent<LissajousMovement>().enabled = false;
        objectA.GetComponent<TrailRenderer>().enabled = false;
        objectB.GetComponent<TrailRenderer>().enabled = false;
        if (objectA.GetComponent<VertexDisplacement>() != null)
        {
            objectA.GetComponent<VertexDisplacement>().enabled = false;
        }
        // Center obj A and point it to the right
        var xrOrigin = GetComponent<XROrigin>();
        objectA.transform.position = xrOrigin.Camera.transform.position + xrOrigin.Camera.transform.forward * 1f;
        objectA.transform.rotation = Quaternion.LookRotation(Vector3.right, Vector3.up);

        // Apply the material with the shader
        var objectArenderer = objectA.GetComponent<Renderer>();
        objectArenderer.material = new Material(colorChangeMaterial);

        rotationSpeedSlider.onValueChanged.AddListener((value) => rotationSpeed = value);
        rotationSpeedSlider.value = rotationSpeed;

        // Start coroutine
        colorChangeRoutine = StartCoroutine(ColorChangeEffect());
    }
    private IEnumerator<WaitForEndOfFrame> ColorChangeEffect()
    {
        float angle = 0f;
        float radius = 0.7f;
        var mat = objectA.GetComponent<Renderer>().material;

        while (true)
        {
            angle += Time.deltaTime * rotationSpeed;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            //spin arond the Z axis
            objectB.transform.position = objectA.transform.position + new Vector3(x, y, 0);

            // Update dot product and send it to shader
            Vector3 dirToB = (objectB.transform.position - objectA.transform.position).normalized;
            float dot = Vector3.Dot(objectA.transform.forward, dirToB);
            mat.SetFloat("_DotProduct", dot);

            yield return null;
        }
    }
    public void SetupVertexAnimationDemo()
    {
        if (objectA == null) { SpawnObjectA(); }
        if (objectB != null) objectB.transform.position=new Vector3(0,-100f,0);// move object B out of view
        TogglePanels(vertexAnimationPanel);

        DisableAutoRotation(objectA);
        DisableAutoRotation(objectB);
        objectA.GetComponent<TrailRenderer>().enabled = false;
        objectB.GetComponent<TrailRenderer>().enabled = false;
        objectA.GetComponent<LissajousMovement>().enabled = false;
        objectB.GetComponent<LissajousMovement>().enabled = false;

        // Stop other routines
        StopAllCoroutines();

        // Position Object A in center of view
        var xrOrigin = GetComponent<XROrigin>();
        objectA.transform.position = xrOrigin.Camera.transform.position + xrOrigin.Camera.transform.forward * 2f;
        objectA.GetComponent<Renderer>().material = VertexAnimationMaterial;

        //Make sure the object has a VertexDisplacement component
        var displacer = objectA.GetComponent<VertexDisplacement>();
        if (displacer == null)
            displacer = objectA.AddComponent<VertexDisplacement>();

        // Sync sliders to values and setup UI bindings
        noiseScrollSpeedSlider.value = displacer.scrollSpeed;
        noiseScaleSlider.value = displacer.noiseScale;
        noiseDisplacementSlider.value = displacer.noiseDisplacement;

        noiseScrollSpeedSlider.onValueChanged.AddListener((val) => displacer.scrollSpeed = val);
        noiseScaleSlider.onValueChanged.AddListener((val) => displacer.noiseScale = val);
        noiseDisplacementSlider.onValueChanged.AddListener((val) => displacer.noiseDisplacement = val);
    }
    public void SetupVRAttractorDemo()
    {
        if (objectA == null) { SpawnObjectA(); }
        if (objectB == null) { SpawnObjectB(); }
        // Deactivate other panels
        TogglePanels(vrAttractorSidePanel);

        // Stop other routines
        StopAllCoroutines();

        DisableAutoRotation(objectA);
        DisableAutoRotation(objectB);
        objectA.GetComponent<LissajousMovement>().enabled = false;
        objectB.GetComponent<LissajousMovement>().enabled = false;
        objectA.GetComponent<TrailRenderer>().enabled = false;
        objectB.GetComponent<TrailRenderer>().enabled = false;
        if (objectA.GetComponent<VertexDisplacement>() != null) {   objectA.GetComponent<VertexDisplacement>().enabled = false; }

        objectA.GetComponent<Renderer>().material = proceduralMeshGenerator.defaultMeshMaterial;
        objectA.GetComponent<Renderer>().material.color = Color.red;
        objectB.GetComponent<Renderer>().material = proceduralMeshGenerator.defaultMeshMaterial;
        objectB.GetComponent<Renderer>().material.color = Color.blue;

        // Move objectA to be directly in front of the camera
        ResetVRAttractionDemo();

        rightControllerattractionForceSlider.onValueChanged.AddListener((value) => rightControllerattractionForce = value);
        leftControllerattractionForceSlider.onValueChanged.AddListener((value) => leftControllerattractionForce = value);
        rightControllerattractionForceSlider.value = rightControllerattractionForce;
        leftControllerattractionForceSlider.value= leftControllerattractionForce;

        vrAttractionRoutine = StartCoroutine(VRAttractionEffect());
    }
    private IEnumerator<WaitForEndOfFrame> VRAttractionEffect()
    {
        float attractionRadius=0.4f;

        while (true)
        {   

            //Attract obj A or B to their respective controllers if they are within distance
            if(Vector3.Distance(objectA.transform.position,rightController.position) < attractionRadius)
            {
                objectA.transform.position = Vector3.Lerp(objectA.transform.position, rightController.transform.position, rightControllerattractionForce * Time.deltaTime);
            }
            if (Vector3.Distance(objectB.transform.position, leftController.position) <= attractionRadius)
            {
                objectB.transform.position = Vector3.Lerp(objectB.transform.position, leftController.transform.position, leftControllerattractionForce * Time.deltaTime);
            }
            yield return null;
        }
    }

    #region Helper Functions
    public void SpawnObjectA()
    {
        if (objectA == null) { objectA = SpawnObject("Object A", new Vector3(-0.4f, 0f, 1f), Color.red); }
    }
    public void SpawnObjectB()
    {
        if (objectB == null) { objectB = SpawnObject("Object B", new Vector3(0.4f, 0f, 1f), Color.blue); }
    }
    private GameObject SpawnObject(string name, Vector3 offset, Color color)
    {
        GameObject obj = proceduralMeshGenerator.GenerateDemoGameobject(name);
        var camTransform = GetComponent<XROrigin>().Camera.transform;

        obj.transform.position = camTransform.position + (camTransform.forward * offset.z) + (camTransform.right * offset.x) + (camTransform.up * offset.y);
        //obj.transform.SetParent(transform);

        obj.AddComponent<LissajousMovement>();
        obj.GetComponent<LissajousMovement>().enabled = false;

        // Trail renderer setup
        var trail = obj.AddComponent<TrailRenderer>();
        var TrailMat = new Material(lissajousTrailMaterial);
        TrailMat.color = color;
        trail.material = TrailMat;
        trail.startColor = color;
        trail.endColor = color;
        trail.startWidth = 0.01f;
        trail.endWidth = 0.01f;
        trail.time = 3f;

        obj.GetComponent<Renderer>().material.color = color;

        // Auto rotate setup
        var autoRotate = obj.AddComponent<AutoRotator>();
        autoRotate.rotationSpeed = 45f;
        autoRotate.isActive = true;

        return obj;
    }
    private void DisableAutoRotation(GameObject obj)
    {
        var rotator = obj.GetComponent<AutoRotator>();
        if (rotator != null)
        {
            rotator.isActive = false;
            obj.transform.rotation = Quaternion.identity;
        }
    }
    private void TogglePanels(GameObject activePanel)
    {
        ToggleAllPanels(false);
        activePanel.SetActive(true);
    }
    private void ToggleAllPanels(bool active)
    {
        LissajousPanel.SetActive(active);
        objectRotationPanel.SetActive(active);
        vertexAnimationPanel.SetActive(active);
        colorChangePanel.SetActive(active);
        vrAttractorSidePanel.SetActive(active);
    }

    public void ResetVRAttractionDemo()
    {
        var xrOrigin = GetComponent<XROrigin>();
        var camTransform = xrOrigin.Camera.transform;
        objectA.transform.position = camTransform.position + (camTransform.forward * 0.3f) + (camTransform.right * -.3f) + (camTransform.up * 0);
        objectB.transform.position = camTransform.position + (camTransform.forward * 0.3f) + (camTransform.right * .3f) + (camTransform.up * 0);
        
    }
    #endregion
}