using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Jam
{
    public sealed class GameManager : MonoBehaviour
    {
        // Singleton :(

        public static GameManager Instance { get; private set; }

        public void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // Scene

        /// <summary>
        /// Loads the credit scene
        /// </summary>
        public void LoadCredits()
        {
            StartCoroutine(LoadSceneWithTransition(1));
        }

        /// <summary>
        /// Loads the main menu scene
        /// </summary>
        public void LoadMainMenu()
        {
            StartCoroutine(LoadSceneWithTransition(0));
        }

        // Offset is menu scene
        private const int k_LevelOffset = 2;

        /// <summary>
        /// Loads a level from its index
        /// </summary>
        public void LoadLevelFromIndex(int index)
        {
            StartCoroutine(LoadSceneWithTransition(index + k_LevelOffset));
        }

        /// <summary>
        /// Returns the name of a level, based on its index
        /// </summary>
        public string NameForLevelFromIndex(int index)
        {
            return index switch {
                0 => "The Sushi Stop",
                1 => "Burrito Problems",
                2 => "The Great Toilet Paper Shortage",
                _ => null
            };
        }

        // Transition

        [SerializeField] private TransitionFader m_Transition;

        private IEnumerator LoadSceneWithTransition(int index)
        {
            yield return m_Transition.ShowTransition();


            yield return SceneManager.LoadSceneAsync(index);

            Reset();

            // Make sure its not too fast...
            yield return new WaitForSeconds(0.6f);
            yield return m_Transition.HideTransition();
        }

        // State

        /// <summary>
        /// The current player game object
        /// </summary>
        public GameObject Player { get; set; }

        /// <summary>
        /// Resets the game to its default state, ready to be played again
        /// </summary>
        public void Reset()
        {
            m_Speed = 1;
        }

        /// <summary>
        /// Show win state / switch to next level
        /// </summary>
        public void Win()
        {
            LoadLevelFromIndex(SceneManager.GetActiveScene().buildIndex - k_LevelOffset + 1);
        }

        // Points

        /// <summary>
        /// The speed of the game, should act as a multiplier
        /// </summary>
        public float Speed => m_Speed;

        private float m_Speed;

        public void IncrementSpeed()
        {
            m_Speed += 0.1f;
        }

        public void DecrementSpeed()
        {
            m_Speed += 0.1f;
        }

        private void Update() { }
    }
}
