using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;
namespace Demo.VRAttractor
{
    [System.Serializable]
    public class VRAttractorHandler 
    {
        public Transform leftController;
        public Transform rightController;
        public float rightControllerattractionForce;
        public float leftControllerattractionForce;
        public float attractionRadius = 0.4f;
        public Slider rightControllerattractionForceSlider;
        public Slider leftControllerattractionForceSlider;

        private GameObject objectA;
        private GameObject objectB;
        private XROrigin xrOrigin;

        public void Initialise(GameObject a, GameObject b,XROrigin xROrigin)
        {
            objectA = a;
            objectB = b;
            xrOrigin = xROrigin;

            //bind sliders
            rightControllerattractionForceSlider.onValueChanged.AddListener((value) => rightControllerattractionForce = value);
            leftControllerattractionForceSlider.onValueChanged.AddListener((value) => leftControllerattractionForce = value);
            rightControllerattractionForceSlider.value = rightControllerattractionForce;
            leftControllerattractionForceSlider.value = leftControllerattractionForce;
        }
        public IEnumerator<WaitForEndOfFrame> VRAttractionEffect()
        {
            while (true)
            {
                //Attract obj A or B to their respective controllers if they are within distance
                if (Vector3.Distance(objectA.transform.position, rightController.position) < attractionRadius)
                {
                    objectA.transform.position = Vector3.Lerp(objectA.transform.position, rightController.transform.position, rightControllerattractionForce * Time.deltaTime);
                }
                if (Vector3.Distance(objectB.transform.position, leftController.position) <= attractionRadius)
                {
                    objectB.transform.position = Vector3.Lerp(objectB.transform.position, leftController.transform.position, leftControllerattractionForce * Time.deltaTime);
                }
                yield return null;
            }
        }

        public void ResetVRAttractionDemo()
        {
            var camTransform = xrOrigin.Camera.transform;
            objectA.transform.position = camTransform.position + (camTransform.forward * 0.3f) + (camTransform.right * -.3f) + (camTransform.up * 0);
            objectB.transform.position = camTransform.position + (camTransform.forward * 0.3f) + (camTransform.right * .3f) + (camTransform.up * 0);
        }
    }
}
