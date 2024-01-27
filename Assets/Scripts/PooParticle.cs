using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(ParticleSystem))]
    public class PooParticle : MonoBehaviour
    {
        [SerializeField]private float minScalar = 0.5f;
        [SerializeField]private float maxScalar = 2f;
        
        [SerializeField]private Vector3 velocityDirection = new Vector3(0, 1, 0);
        [SerializeField]private float minVelocityScale = 0.5f;
        [SerializeField]private float maxVelocityScale = 2f;
        
        private ParticleSystem _particleSystem;
        private Vector3 _initialScale;
        
        private void Start()
        {
            _initialScale = transform.localScale;
            _particleSystem = GetComponent<ParticleSystem>();
        }
        
        public void UpdateViolenceScale(int violence)
        {
            Debug.Log($"Updating violence to {violence}");
            var scale = Mathf.Lerp(minScalar, maxScalar, violence / 100f);
            transform.localScale = _initialScale * scale;
        }
        
        public void UpdateVelocityScale(int velocity)
        {
            // var scale = Mathf.Lerp(minVelocityScale, maxVelocityScale, velocity / 100f);
            // var scalarVector = velocityDirection * scale;
            //
            // // scale the local scale by the scalar vector
            // transform.localScale = Vector3.Scale(_initialScale, Vector3.Scale(scalarVector, _initialScale));            
        }
        
        public void UpdateColor(Color color)
        {
            // var main = _particleSystem.main;
            // main.startColor = color;
        }
    }
}