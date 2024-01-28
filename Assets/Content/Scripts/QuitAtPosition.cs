using UnityEngine;

namespace Jam
{
    public sealed class QuitAtPosition : MonoBehaviour
    {
        [SerializeField] private float m_QuitPosition = 10;

        private bool m_Finished;

        private void Update()
        {
            if (m_Finished)
                return;

            if (!(((RectTransform)transform).anchoredPosition.y > m_QuitPosition))
                return;

            m_Finished = true;
            GameManager.Instance.LoadMainMenu();
            m_Finished = true;
        }
    }
}
