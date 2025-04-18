using UnityEngine;

public class LissajousMovement : MonoBehaviour
{
    [Header("Amplitude Controls")]
    [SerializeField,Range(0, 6)]
    private float amplitudeX = 1f;
    [SerializeField, Range(0, 6)]
    private float amplitudeY = 1f;
    // private float amplitudeZ = 1f;

    [Header("Frequency Controls")]
    [SerializeField, Range(0, 6)]
    private float frequencyX = 1f;
    [SerializeField, Range(0, 6)]
    private float frequencyY = 1f;
    //private float frequencyZ = 1f;

    [Header("Animation Controls")]
    public float delta = 0f;

    [SerializeField, Range(0, 10)]
    public float movementSpeed=1f;

    private float time;                     //interal time tracker
    private Vector3 startPosition;          //start postion
    [SerializeField]
    private bool LissajousMovementEnabled;  //Lissajous Movement Enabled. need to implement acess to other scripts

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition=transform.position;
    }


    void Update()
    {   
        
        
        time += (Time.deltaTime * movementSpeed);   //update time

        //calculate offset based on Lissajous equation on X and Y axis
        float xPos=amplitudeX*Mathf.Sin(frequencyX*time+delta);
        float yPos = amplitudeY * Mathf.Sin(frequencyY * time);
        //float zPos

        if (LissajousMovementEnabled)
        {
            
            transform.position = startPosition + new Vector3(xPos, yPos, 0f);   //update position
        }


    }
}
