using System;
using UnityEngine;

namespace Jam
{
    public enum PickupType { ToiletPaper, Food, Point, Barrier }

    public sealed class Pickup : MonoBehaviour
    {
        public PickupType Type;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<MovementController>(out _))
            {
                Debug.Log("Trigger Enter");
                OnPickup();
            }
        }

        public void OnPickup()
        {
            Debug.Log($"Picking up {Type}");

            switch (Type)
            {
                case PickupType.ToiletPaper :
                    GameManager.Instance.DecrementSpeed();
                    break;
                case PickupType.Food :
                    GameManager.Instance.IncrementSpeed();
                    break;
                case PickupType.Point :
                    Destroy(gameObject);
                    break;
                default :
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
