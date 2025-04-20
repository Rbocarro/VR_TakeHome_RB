using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;

public class DemoManager : MonoBehaviour
{
    [SerializeField]
    private GameObject objectA;
    [SerializeField]
    private GameObject objectB;

    [Header("Lissajous Panel")]
    public GameObject LissajousPanel;
    public Material lissajousTrailMaterial;

    public TMP_Dropdown objectADropdown; // Use TextMeshPro Dropdown
    public Slider objectAFrequencyXSlider;
    public Slider objectAFrequencyYSlider;
    public Slider objectAAmplitudeXSlider;
    public Slider objectAAmplitudeYSlider;
    public Slider objectADeltaSlider;
    public Slider objectAMovementSpeed;
    public Slider objectAAnimationScale;



    [Header("Object Rotation Panel")]
    public GameObject objectRotationPanel;

    [Header("Vertex Animation Panel Settings")]
    public GameObject vertexAnimationPanel;

    private void Start()
    {
        LissajousPanel.SetActive(false);
    }
    public void SetupLissajousDemo()
    {
        if (objectA == null) {  SpawnObjectA(); }
        if (objectB == null) {  SpawnObjectB(); }
        LissajousPanel.SetActive(true);
        var lissajousA = objectA.GetComponent<LissajousMovement>();

        List<string> options = new List<string>(
        System.Enum.GetNames(typeof(LissajousMovement.LissajousCurveType)));
        objectADropdown.ClearOptions();
        objectADropdown.AddOptions(options);

        // Sync dropdown value with current curveType
        objectADropdown.value = (int)lissajousA.curveType;
        objectADropdown.onValueChanged.AddListener((index) =>
        {
            lissajousA.curveType =(LissajousMovement.LissajousCurveType)index;
        });

        

        // Setup slider listeners
        objectAFrequencyXSlider.onValueChanged.AddListener((value) => lissajousA.frequencyX = value);                                                 
        objectAFrequencyYSlider.onValueChanged.AddListener((value) => lissajousA.frequencyY = value);
        objectAAmplitudeXSlider.onValueChanged.AddListener((value) => lissajousA.amplitudeX = value);
        objectAAmplitudeYSlider.onValueChanged.AddListener((value) => lissajousA.amplitudeY = value);
        objectADeltaSlider.onValueChanged.AddListener((value) => lissajousA.phaseAngle = value);
        objectAMovementSpeed.onValueChanged.AddListener((value) => lissajousA.movementSpeed = value);
        objectAAnimationScale.onValueChanged.AddListener((value) => lissajousA.animationScale = value);

        // initialise sliders to match the default Lissajous values
        objectAFrequencyXSlider.value = lissajousA.frequencyX;
        objectAFrequencyYSlider.value = lissajousA.frequencyY;
        objectAAmplitudeXSlider.value = lissajousA.amplitudeX;
        objectAAmplitudeYSlider.value = lissajousA.amplitudeY;
        objectADeltaSlider.value = lissajousA.phaseAngle;
        objectAMovementSpeed.value = lissajousA.movementSpeed;
        objectAAnimationScale.value=lissajousA.animationScale;
    }



    public void SpawnObjectA()
    {   if (objectA == null)
        {
            objectA = GetComponent<ProceduralMeshGenerator>().GenerateDemoGameobject("Object A");
            objectA.transform.position = new Vector3(GetComponent<XROrigin>().Camera.transform.position.x-0.5f,
                                                     GetComponent<XROrigin>().Camera.transform.position.y,
                                                     GetComponent<XROrigin>().Camera.transform.position.z + 0.5f);
            //objectA.transform.SetParent(GetComponent<XROrigin>().Origin.transform);
            objectA.AddComponent<LissajousMovement>();
            objectA.AddComponent<TrailRenderer>();
            objectA.GetComponent<TrailRenderer>().material = lissajousTrailMaterial;
            objectA.GetComponent<TrailRenderer>().startWidth = 0.05f;
            objectA.GetComponent<TrailRenderer>().endWidth = 0.05f;
            objectA.GetComponent<TrailRenderer>().time=1f;

        }
        
      
    }

    public void SpawnObjectB()
    {
        if (objectB == null)
        {
            objectB = GetComponent<ProceduralMeshGenerator>().GenerateDemoGameobject("Object B");
            objectB.transform.position = new Vector3(GetComponent<XROrigin>().Camera.transform.position.x+0.5f,
                                                     GetComponent<XROrigin>().Camera.transform.position.y,
                                                     GetComponent<XROrigin>().Camera.transform.position.z + 0.5f);
            objectB.transform.SetParent(transform);
            objectB.AddComponent<LissajousMovement>();
        }
        
    }

    public void RandomiseLissajousValues()
    {
        objectAFrequencyXSlider.value = Random.Range(objectAFrequencyXSlider.minValue, objectAFrequencyXSlider.maxValue);
        objectAFrequencyYSlider.value = Random.Range(objectAFrequencyYSlider.minValue, objectAFrequencyYSlider.maxValue);
        objectAAmplitudeXSlider.value = Random.Range(objectAAmplitudeXSlider.minValue, objectAAmplitudeXSlider.maxValue);
        objectAAmplitudeYSlider.value = Random.Range(objectAAmplitudeYSlider.minValue, objectAAmplitudeYSlider.maxValue);
        objectADeltaSlider.value = Random.Range(objectADeltaSlider.minValue, objectADeltaSlider.maxValue);
        objectAMovementSpeed.value = Random.Range(objectAMovementSpeed.minValue, objectAMovementSpeed.maxValue);
    }

    public void ResetLissajousValues()
    {
        //TO DO
    }

    public void ObjectRotationDemo()
    { 
        //TO DO
    }
}