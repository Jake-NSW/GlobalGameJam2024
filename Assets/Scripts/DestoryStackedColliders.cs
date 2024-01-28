using System.Collections;
using System.Collections.Generic;
using Jam;
using UnityEngine;

public class DestoryStackedColliders : MonoBehaviour
{
    public PickupType m_pickupType;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pickup"))
        {
            var otherPickup = other.GetComponent<Pickup>();

            if (m_pickupType == PickupType.Barrier)
            {
                Debug.Log("Barrier Kills: " + other.gameObject.name);
                Destroy(other.gameObject);
            }

            if (m_pickupType == PickupType.Food && otherPickup.Type != PickupType.Barrier)
            {
                Debug.Log("Food Kills: " + other.gameObject.name);
                Destroy(other.gameObject);
            }
        }
    }
}
