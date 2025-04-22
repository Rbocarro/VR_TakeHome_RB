using UnityEngine;

public class LissajousMovement : MonoBehaviour
{
    [Header("Amplitude Controls")]
    [SerializeField,Range(0, 6)]
    public float amplitudeX = 1f;
    [SerializeField, Range(0, 6)]
    public float amplitudeY = 1f;
    // private float amplitudeZ = 1f;

    [Header("Frequency Controls")]
    [SerializeField, Range(0, 6)]
    public float frequencyX = 1f;
    [SerializeField, Range(0, 6)]
    public float frequencyY = 1f;
    //private float frequencyZ = 1f;

    [Header("Animation Controls")]
    [SerializeField, Range(0, 5)]
    public float phaseAngle = 0f;
    [SerializeField, Range(0, 10)]
    public float movementSpeed=1f;
    [SerializeField, Range(0, 2)]
    public float animationScale = 0.5f;

    private float time;                     //internal time tracker
    private Vector3 startPosition;          //internal start postion
    private float xPos, yPos;               //internal X, Y position tracker

    public enum LissajousCurveType {Standard,Heart,Butterfly }
    public LissajousCurveType curveType;


    void Start()
    {
        startPosition=transform.position;
        curveType =LissajousCurveType.Standard;
    }
    void Update()
    {
        time += (Time.deltaTime * movementSpeed);   //update time
        switch (curveType)
        {
            case LissajousCurveType.Standard:
                //calculate offset based on standard Lissajous equation on X and Y axis
                xPos = amplitudeX * Mathf.Sin(frequencyX * time + phaseAngle);
                yPos = amplitudeY * Mathf.Sin(frequencyY * time);
                break;

            case LissajousCurveType.Heart:
                //calculate offset based on Heart Lissajous equation on X and Y axis
                xPos =  5 * Mathf.Pow(Mathf.Sin(time), 3);//5 sin3t,
                yPos =  4 * Mathf.Cos(time) - 1.3f * Mathf.Cos(2 * time) - 0.6f * Mathf.Cos(3 * time) - 0.2f * Mathf.Cos(4 * time);
                break;

            case LissajousCurveType.Butterfly:
                float sinT = Mathf.Sin(time);
                float cosT = Mathf.Cos(time);
                float butterflyComponent = Mathf.Exp(cosT) - 2 * Mathf.Cos(4 * time) - Mathf.Pow(Mathf.Sin(time / 12f), 5);
                xPos= amplitudeX * sinT * butterflyComponent;
                yPos= amplitudeY * cosT * butterflyComponent;
                break;
        }

        transform.position = startPosition + (new Vector3(xPos, yPos, 0f)* animationScale);   //update position
    }
}
