using Jam;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CinematicGameManagerHandler : MonoBehaviour
{
    private bool called = false;
    
    public void NextLevel()
    {
        if (called) return;

        called = true;
        
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
