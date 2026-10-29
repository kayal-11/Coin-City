using UnityEngine;

public class RotationController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
     public float rotationSpeed;
     public Vector3 rotationDirection;
     void Update()
     {
        transform.Rotate(rotationDirection * rotationSpeed * Time.deltaTime);
     }
}
