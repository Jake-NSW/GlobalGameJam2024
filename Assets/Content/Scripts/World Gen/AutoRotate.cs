using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    // Rotation speed in degrees per second
    public float rotationSpeed = 45.0f;

    // Update is called once per frame
    void Update()
    {
        // Rotate around the Y axis at rotationSpeed degrees/second
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
