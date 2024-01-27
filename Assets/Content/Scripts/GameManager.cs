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

        public float ToiletScaleStart = 50;
        public float TimeForToiletToSettle = 30;

        private void Update()
        {
            TimeInLevel += Time.deltaTime * SpeedMultiplier;

            // Start with scaling down goal, then once around the 10 second mark use position
            var scale = Vector3.Lerp(Vector3.one * 20, Vector3.one, Mathf.Clamp01((TimeInLevel - ToiletScaleStart) / TimeForToiletToSettle));
            Goal.transform.localScale = scale;

            var offset = (1 - TimeInLevel / 100) * 100;
            Goal.transform.position = new Vector3(offset, 0, 0);
            Goal.transform.position += new Vector3(0, scale.y / 2, 0);
        }
    }
}
