using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;
public class DemoManager : MonoBehaviour
{
    [Header("Demo GameObjects")]
    [SerializeField]
    private GameObject objectA;
    [SerializeField]
    private GameObject objectB;

    [Header("Lissajous Panel")]
    public GameObject LissajousPanel;
    public Material lissajousTrailMaterial;//default placeholder material for LJ demo
    public LissajousUIBindings objectAUI;
    public LissajousUIBindings objectBUI;

    [Header("Object Rotation Panel")]
    public GameObject objectRotationPanel;
    public float angularSpeed;// Object A angilar Rotation Speed
    public float radiusAroundObjectA;//radius around object A where
    public Slider angularSpeedSlider;
    private Coroutine rotationRoutine;

    [Header("Color Change Demo Settings")]
    public float rotationSpeed;

    [Header("Vertex Animation Panel Settings")]
    public GameObject vertexAnimationPanel;

    private void Start()
    {
        //set all side panels active status to false at start
        ToggleAllPanels(false);
    }
    public void SetupLissajousDemo()
    {
        // Deactivate other panels
        ToggleAllPanels(LissajousPanel);

        if (objectA == null) {  SpawnObjectA(); }
        if (objectB == null) {  SpawnObjectB(); }

        var xrOrigin = GetComponent<XROrigin>();
        objectA.transform.position = xrOrigin.Camera.transform.position + new Vector3(-0.5f, 0f, 0.5f);
        objectB.transform.position = xrOrigin.Camera.transform.position + new Vector3(0.5f, 0f, 0.5f);

        // Stop any cureent or prevrunning rotation demo
        if (rotationRoutine != null)
        {
            StopCoroutine(rotationRoutine);
            rotationRoutine = null;
        }

        DisableAutoRotation(objectA);
        DisableAutoRotation(objectB);

        objectA.GetComponent<LissajousMovement>().enabled=true;
        objectB.GetComponent<LissajousMovement>().enabled = true;
        var lissajousA = objectA.GetComponent<LissajousMovement>();
        var lissajousB = objectB.GetComponent<LissajousMovement>();
        objectAUI.BindTo(lissajousA);
        objectBUI.BindTo(lissajousB);
    }
    public void SpawnObjectA()
    {
        if (objectA == null) { objectA = SpawnObject("Object A", new Vector3(-0.5f, 0f, 0.5f), Color.red);}
    }

    public void SpawnObjectB()
    {
        if (objectB == null) { objectB = SpawnObject("Object B", new Vector3(0.5f, 0f, 0.5f), Color.blue); }
    }


    private GameObject SpawnObject(string name, Vector3 offset, Color trailColor)
    {
        GameObject obj = GetComponent<ProceduralMeshGenerator>().GenerateDemoGameobject(name);
        var xrOrigin = GetComponent<XROrigin>();

        obj.transform.position = xrOrigin.Camera.transform.position + offset;
        obj.transform.SetParent(transform);

        obj.AddComponent<LissajousMovement>();
        obj.GetComponent<LissajousMovement>().enabled = false;

        // Trail renderer setup
        var trail = obj.AddComponent<TrailRenderer>();
        var TrailMat = new Material(lissajousTrailMaterial);
        TrailMat.color = trailColor;
        trail.material = TrailMat; 
        trail.startColor = trailColor;
        trail.endColor = trailColor;
        trail.startWidth = 0.05f;
        trail.endWidth = 0.05f;
        trail.time = 1f;

        // Auto rotate setup
        var autoRotate = obj.AddComponent<AutoRotator>();
        autoRotate.rotationSpeed = 45f;
        autoRotate.isActive = true;

        return obj;
    }

    public void ObjectRotationDemo()
    {

        if (objectA == null) { SpawnObjectA(); }
        if (objectB == null) { SpawnObjectB(); }
        objectA.GetComponent<TrailRenderer>().enabled = false;
        objectB.GetComponent<TrailRenderer>().enabled= false;
        objectA.GetComponent<LissajousMovement>().enabled = false;
        objectB.GetComponent<LissajousMovement>().enabled = false;
        DisableAutoRotation(objectA);
        DisableAutoRotation(objectB);

        // Deactivate other panels
        TogglePanels(objectRotationPanel);
        angularSpeedSlider.onValueChanged.AddListener((value) => angularSpeed = value);
        

        // Move objectA to be directly in front of the camera
        var xrOrigin = GetComponent<XROrigin>();
        var cameraTransform = xrOrigin.Camera.transform;
        objectA.transform.position = cameraTransform.position + cameraTransform.forward * 1f;

        // Start theloop
        if (rotationRoutine != null) StopCoroutine(rotationRoutine);
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

                yield return new WaitForEndOfFrame();
            }

        }
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

    public void SetupColorChangeDemo()
    {

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
    }
}