using UnityEngine;
using UnityEngine.SceneManagement;

namespace Jam
{
    public sealed class MenuFunctionality : MonoBehaviour
    {
        public void OpenScene(int scene)
        {
            SceneManager.LoadScene(scene);
        }
        
        public void Quit()
        {
            Application.Quit();
        }
    }

}
