using System;
using UnityEngine;

namespace Jam
{
    public enum PickupType { ToiletPaper, Food, Barrier, EndToilet, SpeedBoost }

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
                case PickupType.Barrier :
                    GameManager.Instance.ResetSpeed();
                    break;
                case PickupType.EndToilet  :
                    GameManager.Instance.WinLevel();
                    break;
                default :
                    throw new ArgumentOutOfRangeException();
            }

            Destroy(gameObject);
            GameManager.Instance.PointsFromType(Type);
            // Play Particle and Sound?
        }
    }
}