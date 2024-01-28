using System.Collections;
using UnityEngine;

namespace Jam
{
    public class FartController : MonoBehaviour
    {        
        [field: SerializeField]
        public bool IsFarting { get; private set; } = false;
        [SerializeField]private ParticleSystem[] fartParticles;
        [SerializeField]private AudioClip[] fartSounds;
        
        [SerializeField]private Vector2 fartIntervals = new Vector2(0.5f, 1f);
        
        [SerializeField]
        private AudioSource audioSource;
        private float _elapsedTime = 0f;
        private float _timeToNextFart = 0f;
        
        private void Start()
        {
            _timeToNextFart = Random.Range(fartIntervals.x, fartIntervals.y);
            
            // Play a sound immediately when the game starts
            if (fartSounds.Length > 0)
            {
                audioSource.PlayOneShot(fartSounds[0]);
            }
        }

        private void OnEnable()
        {
            Fart();
        }


        public void Fart()
        {
            // randomly play one of the fart particles
            var randomIndex = Random.Range(0, fartParticles.Length);
            fartParticles[randomIndex].Play();
            
            // play a random fart sound
            if (fartSounds.Length == 0) return;
            
            var randomSoundIndex = Random.Range(0, fartSounds.Length);
            var clip = fartSounds[randomSoundIndex];
            audioSource.PlayOneShot(clip);
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

#if UNITY_EDITOR
namespace Jam
{
    using UnityEditor;
    [CustomEditor(typeof(FartController))]
    public class FartControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            var fartController = (FartController) target;
            if (GUILayout.Button("Fart"))
            {
                // Play a fart sound
                fartController.Fart();
            }
        }
    }
}
#endif