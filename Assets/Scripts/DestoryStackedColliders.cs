using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryStackedColliders : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger detected with: " + other.gameObject.name);
    }
}
