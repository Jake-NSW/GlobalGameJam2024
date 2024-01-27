using System;
using UnityEngine;

namespace Jam
{
    public enum PickupType
    {
        None, ToiletPaper, HandSanitizer, Point
    }

    public sealed class Pickup : MonoBehaviour
    {
        [SerializeField] private PickupType m_Type;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<MovementController>(out _))
                OnPickup();
        }

        public void OnPickup()
        {
            switch (m_Type)
            {
                case PickupType.None :
                    break;
                case PickupType.ToiletPaper :
                    break;
                case PickupType.HandSanitizer :
                    break;
                case PickupType.Point :
                    GameManager.Instance.Points++;
                    Destroy(gameObject);
                    break;
                default :
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
