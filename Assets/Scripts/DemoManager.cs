using UnityEngine;
using UnityEngine.UI;
using Unity.XR.CoreUtils;
using Demo.Utility;
using Demo.VertexAnimation;
using Demo.LissajousAnimation;
using Demo.ColorChange;
using Demo.ObjectRotation;
using Demo.VRAttractor;
public class DemoManager : MonoBehaviour
{
    [Header("Demo GameObjects")]
    public GameObject objectA;
    public GameObject objectB;

    [Header("Procedural Mesh Settings")]
    public ProceduralMeshGenerator proceduralMeshGenerator;

    [Header("Lissajous Demo Settings")]
    public GameObject LissajousPanel;
    public Material lissajousTrailMaterial;
    public LissajousUIBindings objectAUI;
    public LissajousUIBindings objectBUI;

    [Header("Object Rotation Demo Settings")]
    public GameObject objectRotationPanel;
    public ObjectRotationDemoHandler objectRotationDemoHandler;
    private Coroutine rotationRoutine;

    [Header("Color Change Demo Settings")]
    public ColorChangeDemoHandler colorChangeHandler;
    public GameObject colorChangePanel;
    private Coroutine colorChangeRoutine;
    
    [Header("Vertex Animation Demo Settings")]
    public GameObject vertexAnimationPanel;
    public Slider noiseScrollSpeedSlider;
    public Slider noiseScaleSlider;
    public Slider noiseDisplacementSlider;
    public Material VertexAnimationMaterial;

    [Header("VR Attractor Settings")]
    public GameObject vrAttractorSidePanel;
    public VRAttractorHandler vrAttractorHandler;
    private Coroutine vrAttractionRoutine;

    private Transform sceneCameraTransform;     //camera transform for either VR and Non VR mode
    private bool inVRMode;                      //bool to indicate whether the scene is in VR mode
    private void Start()
    {
        ToggleAllPanels(false);                 //set all side panels active status to false at start
        //SetupUIListners();                    //set up UI Listners for all demos

        // Check to see if in VR scene 
        if (TryGetComponent<XROrigin>(out XROrigin xrOrigin))
        {
            sceneCameraTransform = xrOrigin.Camera.transform;
            inVRMode = true;
        }
        else
        {
            sceneCameraTransform = GetComponent<Camera>().transform;
            inVRMode=false;
            Debug.Log("Not in VR mode");    
        }
    }
    public void SetupLissajousDemo()
    {
        // Deactivate other panels
        TogglePanels(LissajousPanel);

        // Stop other coroutines
        StopAllCoroutines();

        if (objectA == null) {  SpawnObjectA(); }
        if (objectB == null) {  SpawnObjectB(); }
;
        if (inVRMode)
        {
            objectA.transform.position = sceneCameraTransform.position + new Vector3(-0.5f, 0f, 1f);
            objectB.transform.position = sceneCameraTransform.position + new Vector3(0.5f, 0f, 1f);
        }
        else
        {
            objectA.transform.position = sceneCameraTransform.position + new Vector3(-5f, 0f, 5f);
            objectB.transform.position = sceneCameraTransform.position + new Vector3(5f, 0f, 5f);
        }

        DisableAutoRotation(objectA);
        DisableAutoRotation(objectB);

        if (objectA.GetComponent<VertexDisplacement>() != null)
        {
            objectA.GetComponent<VertexDisplacement>().enabled = false;
        }
        
        objectA.GetComponent<LissajousMovement>().enabled=true;
        objectB.GetComponent<LissajousMovement>().enabled = true;
        objectA.GetComponent<TrailRenderer>().enabled = true;
        objectB.GetComponent<TrailRenderer>().enabled = true;
        var lissajousA = objectA.GetComponent<LissajousMovement>();
        var lissajousB = objectB.GetComponent<LissajousMovement>();
        objectAUI.BindTo(lissajousA);
        objectBUI.BindTo(lissajousB);
    }
    public void SetupObjectRotationDemo()
    {
        // Deactivate other panels
        TogglePanels(objectRotationPanel);

        // Stop other coroutines
        StopAllCoroutines();

        if (objectA == null) { SpawnObjectA(); }
        if (objectB == null) { SpawnObjectB(); }
        
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

        objectRotationDemoHandler.Initialise(objectA, objectB);

        if (inVRMode)
        {
            objectA.transform.position = sceneCameraTransform.position + sceneCameraTransform.forward * 1f;
        }
        else
        {
            objectA.transform.position = sceneCameraTransform.position + sceneCameraTransform.forward * 5f;

        }

        // Start coroutine
        rotationRoutine = StartCoroutine(objectRotationDemoHandler.RotateTowardsMovingTarget());
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

        if (inVRMode)
        {
            objectA.transform.position = sceneCameraTransform.position + sceneCameraTransform.forward * 1f;
        }
        else
        {
            objectA.transform.position = sceneCameraTransform.position + sceneCameraTransform.forward * 5f;
        }
        
        objectA.transform.rotation = Quaternion.LookRotation(sceneCameraTransform.right, Vector3.up);

        colorChangeHandler.Initialise(objectA, objectB);
        // Apply the material with the shader
        var objectArenderer = objectA.GetComponent<Renderer>();
        objectArenderer.material = new Material(colorChangeHandler.colorChangeMaterial);

        // Start coroutine
        colorChangeRoutine = StartCoroutine(colorChangeHandler.ColorChangeEffect());
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

        // Position Object A in center of view  2f for VR mode 7f for nonVR mode
        float offset = inVRMode ? 2f : 7f;
        objectA.transform.position = sceneCameraTransform.position + sceneCameraTransform.forward * offset;

        objectA.GetComponent<Renderer>().material = VertexAnimationMaterial;

        //Make sure the object has a VertexDisplacement component
        if (!objectA.TryGetComponent<VertexDisplacement>(out var displacer))
        {
            displacer = objectA.AddComponent<VertexDisplacement>();
        }

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

        vrAttractorHandler.Initialise(objectA, objectB,GetComponent<XROrigin>());
        vrAttractorHandler.ResetVRAttractionDemo();

        vrAttractionRoutine = StartCoroutine(vrAttractorHandler.VRAttractionEffect());
    }
    #region Helper Functions
    public void SpawnObjectA()
    {
        if (objectA == null) 
        {
            Vector3 position = inVRMode ? new Vector3(-0.4f, 0f, 1f) : new Vector3(-4f, 0f, 5f);
            objectA = SpawnObject("Object A", position, Color.red);
        }
    }
    public void SpawnObjectB()
    {
        if (objectB == null) 
        {
            Vector3 position = inVRMode ? new Vector3(0.4f, 0f, 1f) : new Vector3(4f, 0f, 5f);//Postion object based on wether in VR mode or not
            objectB = SpawnObject("Object B", position, Color.blue); 
        }
    }
    private GameObject SpawnObject(string name, Vector3 offset, Color color)
    {
        GameObject obj = proceduralMeshGenerator.GenerateDemoGameobject(name);
        // var camTransform = GetComponent<XROrigin>().Camera.transform;

        //obj.transform.position = camTransform.position + (camTransform.forward * offset.z) + (camTransform.right * offset.x) + (camTransform.up * offset.y);
        obj.transform.position = sceneCameraTransform.transform.position + (sceneCameraTransform.forward * offset.z) + (sceneCameraTransform.right * offset.x) + (sceneCameraTransform.up * offset.y);
        //obj.transform.SetParent(transform);

        obj.AddComponent<LissajousMovement>();
        obj.GetComponent<LissajousMovement>().enabled = false;

        // Trail renderer setup
        SetupTrailRenderer(obj, color);

        obj.GetComponent<Renderer>().material.color = color;

        // Auto rotate setup
        var autoRotate = obj.AddComponent<AutoRotator>();
        autoRotate.rotationSpeed = 45f;
        autoRotate.isActive = true;

        return obj;
    }
    private void DisableAutoRotation(GameObject obj)
    {
        //disable autorotation if enabled
        if (obj.TryGetComponent<AutoRotator>(out var rotator))
        {
            rotator.isActive=false;
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
    public void ResetVRAttractionDemoViaUI()
    {
        vrAttractorHandler.ResetVRAttractionDemo();
    }

    public void SetupTrailRenderer(GameObject gameObject,Color trailColor)
    {
        // Trail renderer setup
        var trail = gameObject.AddComponent<TrailRenderer>();
        trail.material = new Material(lissajousTrailMaterial) { color = trailColor };
        trail.startWidth = trail.endWidth = 0.01f;
        trail.time = 3f;
    }

    public void SetupUIListners()
    {       
        //TODO
        //setup listners for all UI panel elements
        //Lissajous
        //Object Rotation
        //Color Change
        //
    }
    #endregion
}