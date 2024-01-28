using UnityEngine;

namespace Jam
{
    public sealed class QuitAtPosition : MonoBehaviour
    {
        [SerializeField] private float m_QuitPosition = 10;

        private void Update()
        {
            if (((RectTransform)transform).anchoredPosition.y > m_QuitPosition)
            {
                Application.Quit();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.ExitPlaymode();
#endif
            }
        }
    }
}
