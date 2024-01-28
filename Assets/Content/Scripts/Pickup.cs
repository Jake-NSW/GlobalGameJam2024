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
                Debug.Log("Trigger Enter");
                OnPickup();
            }
        }

        public async void OnPickup()
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
                case PickupType.EndToilet:
                    GameManager.Instance.ResetSpeed();
                    await Task.Delay(TimeSpan.FromSeconds(0.2f));
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