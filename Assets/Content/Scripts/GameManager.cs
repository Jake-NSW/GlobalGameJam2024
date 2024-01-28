using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

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
            IsPlaying = false;

            Application.targetFrameRate = 60;
            Time.fixedDeltaTime = 0.01667f;
        }

        private void OnGUI()
        {
            // top middle
            if (!IsPlaying)
            {
                GUI.Label(new Rect(Screen.width / 2 - 50, 10, 100, 20), $"Not Playing!");
                return;
            }

            GUI.Label(new Rect(Screen.width / 2 - 50, 10, 100, 20), $"Points: {Points}");
            GUI.Label(new Rect(Screen.width / 2 - 50, 30, 100, 20), $"Speed: {Speed}");
            GUI.Label(new Rect(Screen.width / 2 - 50, 50, 100, 20), $"Time: {Remaining}");
        }

        private void Update()
        {
            PointsUpdateLoop();
            TimerUpdateLoop();
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
        /// Loads the next level
        /// </summary>
        public void LoadNextLevel()
        {
            var active = SceneManager.GetActiveScene().buildIndex;
            LoadLevelFromIndex(active - k_LevelOffset + 1);
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
            if(SceneManager.GetActiveScene().name != "Cinematic4")
                MusicManager.Instance.FadeOutMusic();

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
        public bool IsPlaying { get; private set; }

        public void AssertPlaying()
        {
            if (!IsPlaying)
                throw new UnityException("Not playing, cannot do this");
        }

        // Playing

        [SerializeField] private float m_LevelLength = 60;

        /// <summary>
        /// Remaining time left in the level
        /// </summary>
        public float Remaining => m_LevelLength - m_SinceStart;

        private TimeSince m_SinceStart;

        private void TimerUpdateLoop()
        {
            if (!IsPlaying)
                return;

            if (Remaining <= 0)
                WinLevel();
        }

        /// <summary>
        /// Start playing the game, call after intro sequence for each level?
        /// </summary>
        public void StartPlaying()
        {
            if (IsPlaying)
                throw new UnityException("Already playing, cannot do this");

            IsPlaying = true;

            // Start Timer
            m_SinceStart = 0;

            Reset();
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
        public void WinLevel()
        {
            IsPlaying = false;

            // Should show an event with some UI instead of this?
            LoadNextLevel();
        }

        /// <summary>
        /// Show lose state
        /// </summary>
        public void LooseLevel()
        {
            IsPlaying = false;

            // Do something
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
        public float MaxSpeed => m_MaxSpeed;
        public float MinSpeed => m_MinSpeed;
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
