using UnityEngine;

namespace Jam
{
    public sealed class MenuFunctionality : MonoBehaviour
    {
        public void PlayGame()
        {
            if (GameManager.Instance == null)
                throw new UnityException("GameManager is null");

            GameManager.Instance.LoadLevelFromIndex(0);
        }

        public void Level(int scene)
        {
            if (GameManager.Instance == null)
                throw new UnityException("GameManager is null");

            GameManager.Instance.LoadLevelFromIndex(scene);
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

}
