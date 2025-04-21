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
    public Material lissajousTrailMaterial;
    public LissajousUIBindings objectAUI;
    public LissajousUIBindings objectBUI;

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
        DisableAutoRotation(objectA);
        DisableAutoRotation(objectB);
        LissajousPanel.SetActive(true);
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
        //TO DO
        //set A object infront of the camera
        //

        if (objectA == null) { SpawnObjectA(); }
        if (objectB == null) { SpawnObjectB(); }
        DisableAutoRotation(objectA);
        DisableAutoRotation(objectB);
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
}