using UnityEngine;

public class AutoRotator : MonoBehaviour
{
    [SerializeField, Range(0, 100f)]
    public float rotationSpeed = 45f; // degrees per second
    public bool isActive = true;

    void Update()
    {
        if (isActive)
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
    }
}
