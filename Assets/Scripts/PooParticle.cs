using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(ParticleSystem))]
    public class PooParticle : MonoBehaviour
    {
        [SerializeField]private float minScale = 0.1f;
        [SerializeField]private float maxScale = 1f;
        
        [SerializeField]private Vector3 velocityDirection = new Vector3(0, 1, 0);
        [SerializeField]private float minVelocityScale = 0.1f;
        [SerializeField]private float midVelocityScale = 0.5f;
        [SerializeField]private float maxVelocityScale = 1f;
        
        private ParticleSystem _particleSystem;
        private Vector3 _initialScale;
        
        private void Start()
        {
            _initialScale = transform.localScale;
            _particleSystem = GetComponent<ParticleSystem>();
        }
        
        public void UpdateViolenceScale(int violence)
        {
            var scale = Mathf.Lerp(minScale, maxScale, violence / 100f);
            transform.localScale = _initialScale * scale;
        }
        
        public void UpdateVelocityScale(int velocity)
        {
            var scale = Mathf.Lerp(minVelocityScale, maxVelocityScale, velocity / 100f);
            var scalarVector = velocityDirection * scale;
            
            // scale the local scale by the scalar vector
            transform.localScale = Vector3.Scale(_initialScale, scalarVector);            
        }
        
        public void UpdateColor(Color color)
        {
            var main = _particleSystem.main;
            main.startColor = color;
        }
        
    }
}