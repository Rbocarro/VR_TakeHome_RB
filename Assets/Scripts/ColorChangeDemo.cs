using UnityEngine;
using System.Collections;

namespace Demo.ColorChange
{   
    public class ColorChangeDemo : MonoBehaviour
    {
        public GameObject objectA;  // ref to obj A
        public GameObject objectB;  // reft to obj B
        public Material colorChangeMaterial;
        public float rotationSpeed = 1.5f;  //rotation speed of obj B around A

        private Coroutine colorChangeRoutine;
        private Material _instanceMaterial;

        public void StartColorChange()
        {
            
            // Assign color change material
            _instanceMaterial = new Material(colorChangeMaterial);
            objectA.GetComponent<Renderer>().material = _instanceMaterial;

            if (colorChangeRoutine != null) StopCoroutine(colorChangeRoutine);
            colorChangeRoutine = StartCoroutine(ColorChangeEffect());
        }

        public void StopColorChange()
        {
            if (colorChangeRoutine != null) StopCoroutine(colorChangeRoutine);
            objectA.GetComponent<Renderer>().material.color=Color.red;
            objectB.GetComponent<Renderer>().material.color = Color.blue;
        }

        private IEnumerator ColorChangeEffect()
        {
            float angle = 0f;
            float radius = 0.7f;

            while (true)
            {
                angle += Time.deltaTime * rotationSpeed;
                float x = Mathf.Cos(angle) * radius;
                float y = Mathf.Sin(angle) * radius;

                objectB.transform.position = objectA.transform.position + new Vector3(x, y, 0);

                //update B to also face A continously
                //Vector3 dirToA = (objectA.transform.position - objectB.transform.position).normalized;
                //if (dirToA.sqrMagnitude > 0f)
                //{
                //    Quaternion targetRot = Quaternion.LookRotation(dirToA, Vector3.up);
                //    objectB.transform.rotation = Quaternion.Slerp(objectB.transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
                //}

                // Update dot product and send it to shader
                Vector3 dirToB = (objectB.transform.position - objectA.transform.position).normalized;
                float dot = Vector3.Dot(objectA.transform.forward, dirToB);
                _instanceMaterial.SetFloat("_DotProduct", dot);

                yield return new WaitForEndOfFrame();
            }
        }
        public void SetRotationSpeed(float value)
        {
            rotationSpeed = value;
        }
    }
}
