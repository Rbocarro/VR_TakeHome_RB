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
        public float rotationRadius;//orbit radius around A
        private GameObject objectA;
        private GameObject objectB;
        //private XROrigin xrOrigin;

        public void Initialise(GameObject a, GameObject b)
        {
            objectA = a;
            objectB = b;
        }

        public IEnumerator<WaitForEndOfFrame> ColorChangeEffect()
        {
            float angle = 0f;
            var mat = objectA.GetComponent<Renderer>().material;//obj A material instance

            while (true)
            {
                angle += Time.deltaTime * rotationSpeed;
                float x = Mathf.Cos(angle) * rotationRadius;    //horizontal pos
                float y = Mathf.Sin(angle) * rotationRadius;    //vertical pos

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
