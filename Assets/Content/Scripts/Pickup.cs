using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Jam
{
    public enum PickupType { ToiletPaper, Food, Point, Barrier }

    public sealed class Pickup : MonoBehaviour
    {
        public PickupType Type;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<MovementController>(out _))
                OnPickup();
        }

        public void OnPickup()
        {
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
