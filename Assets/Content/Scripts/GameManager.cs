using System;
using UnityEngine;

namespace Jam
{
    public sealed class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public ToiletGoal Goal;

        public void Awake()
        {
            Instance = this;
        }

        public int Points;

        public float SpeedMultiplier = 1;

        public float TimeInLevel;

        private void Start()
        {
            TimeInLevel = 0;
            Goal.transform.position = new Vector3(0, 0, 100);
        }

        private void Update()
        {
            TimeInLevel += Time.deltaTime * SpeedMultiplier;

            // Start with scaling down goal, then once around the 10 second mark use position
            var scale = Vector3.Lerp(Vector3.one * 50, Vector3.one, Mathf.Clamp01((TimeInLevel - 50) / 50));
            Goal.transform.localScale = scale;
            Goal.transform.position = new Vector3(0, 0, (1 - (TimeInLevel / 100)) * 100);
            Goal.transform.position += new Vector3(0, scale.y / 2, 0);
        }
    }
}
