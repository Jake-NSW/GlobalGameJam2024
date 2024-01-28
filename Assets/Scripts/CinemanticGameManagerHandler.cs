using Jam;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CinematicGameManagerHandler : MonoBehaviour
{
    public void NextLevel()
    {
        if (SceneManager.GetActiveScene().name == "Cinematic4")
        {
            GameManager.Instance.LoadCredits();
        }
        else
        {
            GameManager.Instance.LoadNextLevel();
        }
    }
}
