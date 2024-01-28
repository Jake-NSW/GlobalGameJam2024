using System.Collections;
using UnityEngine;

namespace Jam
{
    [RequireComponent(typeof(AudioSource))]
    public class FartController : MonoBehaviour
    {
        [SerializeField]private ParticleSystem[] fartParticles;
        [SerializeField]private AudioClip[] fartSounds;
        
        [SerializeField]private Vector2 fartIntervals = new Vector2(0.5f, 1f);
        
        private AudioSource _audioSource;
        private float _elapsedTime = 0f;
        private float _timeToNextFart = 0f;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            _timeToNextFart = Random.Range(fartIntervals.x, fartIntervals.y);
        }

        [field: SerializeField]
        public bool IsFarting { get; private set; } = false;

        public void Fart()
        {
            // randomly play one of the fart particles
            var randomIndex = Random.Range(0, fartParticles.Length);
            fartParticles[randomIndex].Play();
            
            // play a random fart sound
            if (fartSounds.Length == 0) return;
            
            var randomSoundIndex = Random.Range(0, fartSounds.Length);
            _audioSource.clip = fartSounds[randomSoundIndex];
            _audioSource.Play();
        }
        
        public void StartFarting()
        {
            IsFarting = true;
        }
        
        public void StopFarting()
        {
            IsFarting = false;
        }

        private void Update()
        {
            if(!IsFarting) return;
            
            _elapsedTime += Time.deltaTime;

            if (!(_elapsedTime >= _timeToNextFart)) return;
            
            Fart();
            _elapsedTime = 0f;
            _timeToNextFart = Random.Range(fartIntervals.x, fartIntervals.y);
        }
    }
}