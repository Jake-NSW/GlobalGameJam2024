using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Jam
{
    [DefaultExecutionOrder(500)] // Register after game manager
    public sealed class GameUI : MonoBehaviour
    {
        public static GameUI Instance { get; set; }

        [SerializeField] private TextMeshProUGUI m_Points;
        [SerializeField] private CanvasGroup m_Root;
        [SerializeField] private Slider m_Fart;
        [SerializeField] private Slider m_Speed;
        [SerializeField] private Slider m_Time;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Update()
        {
            m_Root.blocksRaycasts = false;
            m_Root.alpha = GameManager.Instance.IsPlaying ? 1 : 0;

            m_Points.text = GameManager.Instance.Points.ToString(CultureInfo.InvariantCulture);
            m_Time.value = GameManager.Instance.RemainingNormalized;
            m_Speed.value = GameManager.Instance.Speed / GameManager.Instance.MaxSpeed;
        }

        public float FartPower
        {
            set => m_Fart.value = value;
        }
    }
}
