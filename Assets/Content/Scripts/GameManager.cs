using System;
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

        private void Start()
        {
            Reset();
        }

        private void OnGUI()
        {
            // top middle
            GUI.Label(new Rect(Screen.width / 2 - 50, 10, 100, 20), $"Points: {Points}");
        }

        private void Update()
        {
            PointsUpdateLoop();
        }

        // Scene

        /// <summary>
        /// Loads the credit scene
        /// </summary>
        public void LoadCredits()
        {
            IsPlaying = false;
            StartCoroutine(LoadSceneWithTransition(1));
        }

        /// <summary>
        /// Loads the main menu scene
        /// </summary>
        public void LoadMainMenu()
        {
            IsPlaying = false;
            StartCoroutine(LoadSceneWithTransition(0));
        }

        // Offset is menu scene
        private const int k_LevelOffset = 2;

        /// <summary>
        /// Loads a level from its index
        /// </summary>
        public void LoadLevelFromIndex(int index)
        {
            IsPlaying = true;
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
        /// Are we currently playing the game?
        /// </summary>
        [field: SerializeField]
        public bool IsPlaying { get; set; }

        public void AssertPlaying()
        {
            if (!IsPlaying)
                throw new UnityException("Not playing, cannot do this");
        }

        /// <summary>
        /// Resets the game to its default state, ready to be played again
        /// </summary>
        public void Reset()
        {
            m_Speed = m_MinSpeed;
            m_Points = 0;
            m_PointsUpdateTimer = 0;
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
        /// How many points have been earned
        /// </summary>
        public float Points => m_Points;

        private float m_Points;

        [SerializeField] private float m_PointsPerSecond = 1;
        [SerializeField] private float m_PointsPerFood = 5;
        [SerializeField] private float m_PointsPerToiletPaper = -10;
        [SerializeField] private float m_PointsPerBarrier = -25;

        public void PointsFromType(PickupType type)
        {
            AssertPlaying();

            m_Points += type switch {
                PickupType.Food => m_PointsPerFood,
                PickupType.ToiletPaper => m_PointsPerToiletPaper,
                PickupType.Barrier => m_PointsPerBarrier,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
            
            if (m_Points < 0)
                m_Points = 0;
        }

        private TimeSince m_PointsUpdateTimer;

        private void PointsUpdateLoop()
        {
            // If not playing, set to nothing (lazy!)
            if (!IsPlaying)
                m_PointsUpdateTimer = 0;

            // Add Points every update
            if (m_PointsUpdateTimer <= 1)
                return;

            m_Points += m_PointsPerSecond;
            m_PointsUpdateTimer = 0;
        }

        // Speed

        /// <summary>
        /// The speed of the game, should act as a multiplier
        /// </summary>
        public float Speed => m_Speed;

        [SerializeField] private float m_MaxSpeed = 3;
        [SerializeField] private float m_MinSpeed = 1;

        [SerializeField] private float m_DecrementSpeed = 0.5f;
        [SerializeField] private float m_IncrementSpeed = 0.1f;

        private float m_Speed = 1;

        public void IncrementSpeed()
        {
            AssertPlaying();
            m_Speed = Mathf.Clamp(m_Speed + m_IncrementSpeed, m_MinSpeed, m_MaxSpeed);
        }

        public void DecrementSpeed()
        {
            AssertPlaying();
            m_Speed = Mathf.Clamp(m_Speed - m_DecrementSpeed, m_MinSpeed, m_MaxSpeed);
        }

        public void ResetSpeed()
        {
            m_Speed = m_MinSpeed;
        }
    }
}
