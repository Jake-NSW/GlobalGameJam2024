using System;
using UnityEngine;

namespace Jam
{
    public enum PickupType { ToiletPaper, Food, Point }

    public sealed class Pickup : MonoBehaviour
    {
        [SerializeField] private PickupType m_Type;

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
            Debug.Log($"Picking up {m_Type}");

            switch (m_Type)
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
