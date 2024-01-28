using UnityEngine;

namespace Jam
{
    public class PooStreamHandler : MonoBehaviour
    {
        [SerializeField]private PooStream pooStream;
        [SerializeField]private FartController fart;
        
        [SerializeField]private MovementController move;

        [SerializeField]private int minPooViolence = 50;

        private void Start()
        {
            pooStream.gameObject.SetActive(false);
            fart.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (move.IsFarting)
            {
                HandleFart();
                return;
            }
            
            HandlePoo();
        }

        private void HandlePoo()
        {
            if (!pooStream.gameObject.activeSelf)
                pooStream.gameObject.SetActive(true);

            
            if(fart.gameObject.activeSelf)
                fart.gameObject.SetActive(false);
            
            
            // set the poo violence to be equal to current velocity / max velocity + min poo violence
            var range = 100 - minPooViolence;
            var minSpeed = GameManager.Instance.MinSpeed;
            var maxSpeed = GameManager.Instance.MaxSpeed;
            var speedRange = maxSpeed - minSpeed;
            var speed = GameManager.Instance.Speed;
            var speedPercentage = (speed - minSpeed) / speedRange;
            // poo violence will be mapped to the speed percentage so that 0 = 50 and 1 = 100
            var pooViolence = (int)(speedPercentage * (100 - minPooViolence) + minPooViolence);
            pooStream.UpdateViolence(pooViolence);
            
        }

        private void HandleFart()
        {
            if (pooStream.gameObject.activeSelf)
                pooStream.gameObject.SetActive(false);
            
            if(!fart.gameObject.activeSelf)
                fart.gameObject.SetActive(true);
        }
    }
}