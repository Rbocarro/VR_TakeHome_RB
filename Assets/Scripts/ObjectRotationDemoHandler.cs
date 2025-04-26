using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Demo.ObjectRotation
{
    [System.Serializable]
    public class ObjectRotationDemoHandler 
    {
        public float angularSpeed;  // Object A angular Rotation Speed
        public float radiusAroundObjectA;//radius around object A where obj B spawns around
        public Slider angularSpeedSlider;

        private GameObject objectA; //obj A ref
        private GameObject objectB; //obj B ref
        public void Initialise(GameObject a, GameObject b) 
        {
            objectA = a;
            objectB = b;
        }
        public IEnumerator<WaitForEndOfFrame> RotateTowardsMovingTarget()
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

                    yield return null;
                }
            }
        }

        public void RemoveAllListeners()
        {
            angularSpeedSlider.onValueChanged.RemoveAllListeners();
        }
    }
}

