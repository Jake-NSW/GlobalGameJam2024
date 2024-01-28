using UnityEngine;

namespace Jam
{
    public class PooStreamHandler : MonoBehaviour
    {
        [SerializeField]private PooStream pooStream;
        [SerializeField]private MovementController move;

        [SerializeField] private int minPooViolence = 50;
        
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
            // set the poo violence to be equal to current velocity / max velocity + min poo violence
            var range = 100 - minPooViolence;
            pooStream.UpdateViolence((int)((move.Velocity.magnitude / move.MaxSpeed) * range) + minPooViolence);
        }

        private void HandleFart()
        {
            Debug.Log("fart for now");
        }
    }
}