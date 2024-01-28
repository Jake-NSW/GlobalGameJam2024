using System;
using System.Threading.Tasks;
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
                OnPickup();
            }
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
                case PickupType.Barrier :
                    GameManager.Instance.ResetSpeed();
                    break;
                case PickupType.EndToilet:
                    GameManager.Instance.ResetSpeed();
                    GameManager.Instance.WinLevel();
                    break;
                case PickupType.SpeedBoost:
                    GameManager.Instance.SpeedBoost();
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