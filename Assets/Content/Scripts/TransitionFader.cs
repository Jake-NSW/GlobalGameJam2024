using System;
using System.Collections;
using UnityEngine;

namespace Jam
{
    public sealed class TransitionFader : MonoBehaviour
    {
        [SerializeField] private bool m_StartTransitioning;
        [SerializeField] private float m_FadeTime = 0.4f;
        [SerializeField] private CanvasGroup m_Background;

        private void Start()
        {
            m_Background.alpha = m_StartTransitioning ? 1 : 0;
            m_Background.blocksRaycasts = m_StartTransitioning;
        }

        private bool m_State;

        /// <returns> An IEnumerator that is usable in a Coroutine </returns>
        public IEnumerator ShowTransition()
        {
            m_State = true;

            var start = Time.time;

            while (Time.time - start < m_FadeTime)
            {
                if (!m_State)
                    break;

                var time = (Time.time - start) / m_FadeTime;
                m_Background.alpha = Mathf.Lerp(0, 1, time);
                yield return null;
            }

            m_Background.blocksRaycasts = true;
        }

        /// <returns> An IEnumerator that is usable in a Coroutine </returns>
        public IEnumerator HideTransition()
        {
            m_State = false;

            var start = Time.time;
            while (Time.time - start < m_FadeTime)
            {
                // No longer needed...
                if (m_State)
                    break;

                var time = (Time.time - start) / m_FadeTime;
                m_Background.alpha = Mathf.Lerp(1, 0, time);
                yield return null;
            }

            m_Background.blocksRaycasts = false;
        }
    }
}
