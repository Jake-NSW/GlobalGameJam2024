using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace Jam
{
    public sealed class GameUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_Points;
        [SerializeField] private TextMeshProUGUI m_Time;
        [SerializeField] private CanvasGroup m_Root;

        private void Update()
        {
            m_Root.blocksRaycasts = false;
            m_Root.alpha = GameManager.Instance.IsPlaying ? 1 : 0;

            m_Points.text = GameManager.Instance.Points.ToString(CultureInfo.InvariantCulture);
            m_Time.text = GameManager.Instance.Remaining.ToString("0.00");
        }
    }

}
