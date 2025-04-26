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
    public LissajousUIBindings objectAUI;       //Object A Lissajous UI Elements
    public LissajousUIBindings objectBUI;       //Object B Lissajous UI Elements
    private bool LissajousUIListenersSetUp = false;

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
    private bool VertexAnimationListenersSetUp = false;

    [Header("VR Attractor Settings")]
    public GameObject vrAttractorSidePanel;
    public VRAttractorHandler vrAttractorHandler;
    private Coroutine vrAttractionRoutine;

    private Transform sceneCameraTransform;     //camera transform for either VR and Non VR mode
    private bool inVRMode;                      //bool to indicate whether the scene is in VR mode
    private void Start()
    {
        SetupUIListeners();                      //set up UI Listners for all demos
        ToggleAllPanels(false);                 //set all side panels active status to false at start
        
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
        }
    }
    public void SetupLissajousDemo()
    {
        TogglePanels(LissajousPanel);       // Deactivate other panels
        StopAllCoroutines();                //Stop other coroutines

        
        if (objectA == null) {  SpawnObjectA(); }
        if (objectB == null) {  SpawnObjectB(); }

        //Setup Objects for Lissajous Demo
        EnableLissajousComponents(objectA);
        EnableLissajousComponents(objectB);
        SetUpObjectMaterialandColor(objectA, Color.red);
        SetUpObjectMaterialandColor(objectB, Color.blue);
        if (objectA.TryGetComponent<VertexDisplacement>(out var displacement)) { displacement.enabled = false; }

        // Position Object A & B for VR or nonVR mode
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

        //Sync sliders to values and setup UI button bindings only the first time
        if(!LissajousUIListenersSetUp)
        {
            var lissajousA = objectA.GetComponent<LissajousMovement>();
            var lissajousB = objectB.GetComponent<LissajousMovement>();
            objectAUI.BindTo(lissajousA);
            objectBUI.BindTo(lissajousB);
            LissajousUIListenersSetUp = true;
        }
    }
    public void SetupObjectRotationDemo()
    {
        TogglePanels(objectRotationPanel);      // Deactivate other panels
        StopAllCoroutines();                    // Stop other coroutines

        //Spawn Objects for demo if havent been spawned
        if (objectA == null) { SpawnObjectA(); }
        if (objectB == null) { SpawnObjectB(); }

        //Setup Objects for Rotation Demo
        DisableLissajousComponents(objectA);
        DisableLissajousComponents(objectB);
        SetUpObjectMaterialandColor(objectA, Color.red);
        SetUpObjectMaterialandColor(objectB, Color.blue);
        if (objectA.TryGetComponent<VertexDisplacement>(out var displacement)) { displacement.enabled = false; }

        // Position Object A & B for VR or nonVR mode
        if (inVRMode)
        {
            objectA.transform.position = sceneCameraTransform.position + sceneCameraTransform.forward * 1f;
        }
        else
        {
            objectA.transform.position = sceneCameraTransform.position + sceneCameraTransform.forward * 5f;
        }

        objectRotationDemoHandler.Initialise(objectA, objectB);                                     //initialise values for coroutine
        rotationRoutine = StartCoroutine(objectRotationDemoHandler.RotateTowardsMovingTarget());    // Start coroutine
    }
    public void SetupColorChangeDemo()
    {   
        TogglePanels(colorChangePanel);     // Deactivate other panels
        StopAllCoroutines();                // Stop other routines

        //Spawn Objects for demo if havent been spawned
        if (objectA == null) { SpawnObjectA(); }
        if (objectB == null) { SpawnObjectB(); }

        //Setup Objects for Color Change Demo
        DisableLissajousComponents(objectA);
        DisableLissajousComponents(objectB);
        if (objectA.TryGetComponent<VertexDisplacement>(out var displacement)) { displacement.enabled = false; }

        // Position Object A for VR or nonVR mode
        if (inVRMode)
        {
            objectA.transform.position = sceneCameraTransform.position + sceneCameraTransform.forward * 1f;
        }
        else
        {
            objectA.transform.position = sceneCameraTransform.position + sceneCameraTransform.forward * 5f;
        }
        objectA.transform.rotation = Quaternion.LookRotation(sceneCameraTransform.right, Vector3.up);

        // Apply the material with the shader
        var objectArenderer = objectA.GetComponent<Renderer>();
        objectArenderer.material = new Material(colorChangeHandler.colorChangeMaterial);

        colorChangeHandler.Initialise(objectA, objectB);                                //initialise values for coroutine
        colorChangeRoutine = StartCoroutine(colorChangeHandler.ColorChangeEffect());    // Start coroutine
    }
    public void SetupVertexAnimationDemo()
    {   
        TogglePanels(vertexAnimationPanel);     // Deactivate other panels
        StopAllCoroutines();                    // Stop other routines

        //Spawn Objects for Vertex Anim demo if havent been spawned
        if (objectA == null) { SpawnObjectA(); }
        if (objectB == null) { SpawnObjectB(); }

        //Setup Objects for Color Change Demo
        DisableLissajousComponents(objectA);
        DisableLissajousComponents(objectB);
        if (!objectA.TryGetComponent<VertexDisplacement>(out var displacer)){displacer = objectA.AddComponent<VertexDisplacement>(); }//Make sure the object A has a VertexDisplacement component
        objectA.GetComponent<Renderer>().material = VertexAnimationMaterial;    //Change Object A mat to a Fresnel Mat
        objectB.transform.position = new Vector3(0, -1000f, 0);                 // move object B out of view

        // Position Object A for VR or nonVR mode
        float offset = inVRMode ? 2f : 7f;
        objectA.transform.position = sceneCameraTransform.position + sceneCameraTransform.forward * offset;

        //Sync sliders to values and setup UI button bindings only the first time
        if (!VertexAnimationListenersSetUp)
        {
            noiseScrollSpeedSlider.value = displacer.scrollSpeed;
            noiseScaleSlider.value = displacer.noiseScale;
            noiseDisplacementSlider.value = displacer.noiseDisplacement;
            noiseScrollSpeedSlider.onValueChanged.AddListener((val) => displacer.scrollSpeed = val);
            noiseScaleSlider.onValueChanged.AddListener((val) => displacer.noiseScale = val);
            noiseDisplacementSlider.onValueChanged.AddListener((val) => displacer.noiseDisplacement = val);
        }
    }
    public void SetupVRAttractorDemo()
    {   
        TogglePanels(vrAttractorSidePanel);     // Deactivate other panels
        StopAllCoroutines();                    // Stop other routines

        if (objectA == null) { SpawnObjectA(); }
        if (objectB == null) { SpawnObjectB(); }
        DisableLissajousComponents(objectA);
        DisableLissajousComponents(objectB);
        SetUpObjectMaterialandColor(objectA, Color.red);
        SetUpObjectMaterialandColor(objectB, Color.blue);

        if (objectA.GetComponent<VertexDisplacement>() != null) {   objectA.GetComponent<VertexDisplacement>().enabled = false; }


        // Sync sliders to values and setup UI button bindings
        vrAttractorHandler.Initialise(objectA, objectB,GetComponent<XROrigin>());       //initialise values for coroutine
        vrAttractorHandler.ResetVRAttractionDemo();                                     //Reset Objects 
        vrAttractionRoutine = StartCoroutine(vrAttractorHandler.VRAttractionEffect());  // Start coroutine
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

        obj.transform.position = sceneCameraTransform.transform.position + (sceneCameraTransform.forward * offset.z) + (sceneCameraTransform.right * offset.x) + (sceneCameraTransform.up * offset.y);

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
    public void SetupUIListeners()
    {
        //setup listners for  UI panel elements

        //Lissajous Sliders
        //TODO

        //Object Rotation Sliders
        objectRotationDemoHandler.angularSpeedSlider.onValueChanged.AddListener((value) => objectRotationDemoHandler.angularSpeed = value);
        objectRotationDemoHandler.angularSpeedSlider.value = objectRotationDemoHandler.angularSpeed;

        //Color Change Sliders
        colorChangeHandler.rotationSpeedSlider.onValueChanged.AddListener((value) => colorChangeHandler.rotationSpeed = value);
        colorChangeHandler.rotationSpeedSlider.value = colorChangeHandler.rotationSpeed;

        //VertexAnimation
        //TODO

        //VRAttraction Sliders
        vrAttractorHandler.rightControllerattractionForceSlider.onValueChanged.AddListener((value) => vrAttractorHandler.rightControllerattractionForce = value);
        vrAttractorHandler.leftControllerattractionForceSlider.onValueChanged.AddListener((value) => vrAttractorHandler.leftControllerattractionForce = value);
        vrAttractorHandler.rightControllerattractionForceSlider.value = vrAttractorHandler.rightControllerattractionForce;
        vrAttractorHandler.leftControllerattractionForceSlider.value = vrAttractorHandler.leftControllerattractionForce;

    }
    private void DisableLissajousComponents(GameObject gameObject)
    {  
        //Disable components needed only for Lissajous demo
        gameObject.GetComponent<LissajousMovement>().enabled = false;
        gameObject.GetComponent<TrailRenderer>().enabled = false;
        DisableAutoRotation(gameObject);
    }
    private void EnableLissajousComponents(GameObject gameObject)
    {
        //Enable components needed only for Lissajous demo
        gameObject.GetComponent<LissajousMovement>().enabled = true;
        gameObject.GetComponent<TrailRenderer>().enabled = true;
        DisableAutoRotation(gameObject);
    }
    private void SetUpObjectMaterialandColor(GameObject obj, Color color)
    {   //Setup Material color for object. useful in some demos
        Renderer renderer = obj.GetComponent<Renderer>();
        renderer.material = proceduralMeshGenerator.defaultMeshMaterial;
        renderer.material.color = color;
    }
    #endregion
}