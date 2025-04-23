using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Demo.ColorChange
{
    [System.Serializable]
    public class ColorChangeDemoHandler 
    {
        public Slider rotationSpeedSlider;
        public Material colorChangeMaterial;

        public float rotationSpeed;
        private GameObject objectA;
        private GameObject objectB;
        //private XROrigin xrOrigin;

        public void Initialise(GameObject a, GameObject b)
        {
            objectA = a;
            objectB = b;
            //xrOrigin = origin;

            rotationSpeedSlider.onValueChanged.AddListener((value) => rotationSpeed = value);
            rotationSpeedSlider.value = rotationSpeed;
        }

        public IEnumerator<WaitForEndOfFrame> ColorChangeEffect()
        {
            float angle = 0f;
            float radius = 0.7f;//orbit radius around A
            var mat = objectA.GetComponent<Renderer>().material;//obj A material instance

            while (true)
            {
                angle += Time.deltaTime * rotationSpeed;
                float x = Mathf.Cos(angle) * radius;    //horizontal pos
                float y = Mathf.Sin(angle) * radius;    //vertical pos

                //spin arond the Z axis
                objectB.transform.position = objectA.transform.position + new Vector3(x, y, 0);

                // Update dot product and send it to shader
                Vector3 dirToB = (objectB.transform.position - objectA.transform.position).normalized;
                float dot = Vector3.Dot(objectA.transform.forward, dirToB);
                mat.SetFloat("_DotProduct", dot);

                yield return null;
            }
        }

        public void RemoveAllListeners()
        {
            rotationSpeedSlider.onValueChanged.RemoveAllListeners();
        }

    }
}
